using Locamart.Liam.Application.Contracts.Dtos.User;
using Locamart.Liam.Application.Contracts.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace Locamart.Liam.Adapter.Postgresql;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLiamPostgresServices(this IServiceCollection services, IConfiguration configuration)
    {
        var signingCert = configuration["CertificateSettings:SigningCert"];
        var signingKey = configuration["CertificateSettings:SigningKey"];
        var encryptionCert = configuration["CertificateSettings:EncryptionCert"];
        var encryptionKey = configuration["CertificateSettings:EncryptionKey"];

        var keyPath = configuration["CertificateSettings:KeyPath"];

        if (signingCert is null || signingKey is null || encryptionCert is null || encryptionKey is null)
            throw new InvalidOperationException("Certificate path not found");


        var signCert = X509Certificate2.CreateFromPemFile(signingCert, signingKey);
        var encCert = X509Certificate2.CreateFromPemFile(encryptionCert, encryptionKey);

        services.AddDbContext<LiamDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Liam");
            options.UseNpgsql(connectionString);
            options.UseOpenIddict();
        });

        services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedPhoneNumber = true;
            })
            .AddEntityFrameworkStores<LiamDbContext>()
            .AddDefaultTokenProviders();

        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<LiamDbContext>();
            })
            .AddServer(options =>
            {

                options.SetTokenEndpointUris("connect/token")
                    .SetRevocationEndpointUris("connect/revoke")
                    .SetRefreshTokenLifetime(TimeSpan.FromHours(12))
                    .AllowRefreshTokenFlow()
                    .AllowCustomFlow("otp")
                    .RegisterScopes("api", OpenIddictConstants.Scopes.OfflineAccess)
                    .AddDevelopmentSigningCertificate()
                    .AddDevelopmentEncryptionCertificate()
                    /*                    .AddSigningCertificate(signCert)
                                        .AddEncryptionCertificate(encCert)*/
                    .UseAspNetCore();

                options.AddEventHandler<OpenIddictServerEvents.HandleTokenRequestContext>(builder =>
                {
                    builder.UseInlineHandler(async context =>
                    {
                        if (context.Request.GrantType != "otp")
                            return;

                        var userId = context.Request.GetParameter("username")?.ToString();
                        var otpCode = context.Request.GetParameter("otp_code")?.ToString();
                        var challengeId = context.Request.GetParameter("challenge_id")?.ToString();

                        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(otpCode) || string.IsNullOrEmpty(challengeId))
                        {
                            context.Reject(
                                error: OpenIddictConstants.Errors.InvalidRequest,
                                description: "The user_id, otp_code and challenge_id parameters are required.");
                            return;
                        }

                        var httpContext = context.Transaction.GetHttpRequest()?.HttpContext;

                        if (httpContext is null)
                        {
                            context.Reject(
                                error: OpenIddictConstants.Errors.ServerError,
                                description: "Cannot access HTTP context.");
                            return;
                        }

                        var cacheService = httpContext.RequestServices.GetRequiredService<ICacheService>();
                        var userManager = httpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
                        var signInManager = httpContext.RequestServices.GetRequiredService<SignInManager<IdentityUser>>();



                        var user = await userManager.FindByNameAsync(userId);

                        if (user == null)
                        {
                            var jsonTempUser = await cacheService.GetAsync($"login_challenge:{userId}");

                            if (jsonTempUser.IsFailure)
                            {
                                context.Reject(
                                    error: OpenIddictConstants.Errors.InvalidClient,
                                    description: jsonTempUser.Error.ToString()
                                );
                                return;
                            }

                            if (jsonTempUser.Value is null)
                            {
                                context.Reject(
                                    error: OpenIddictConstants.Errors.InvalidClient,
                                    description: "No login challenge requested"
                                );
                                return;
                            }

                            var tempUser = JsonSerializer.Deserialize<LoginChallengeModel>(jsonTempUser.Value!);

                            if (tempUser is null)
                            {
                                context.Reject(
                                    error: OpenIddictConstants.Errors.ServerError,
                                    description: "Unable to process login challenge on server side"
                                );
                                return;
                            }

                            var removeChallengeResult = await cacheService.RemoveAsync($"login_challenge:{userId}");
                            if (removeChallengeResult.IsFailure)
                            {
                                context.Reject(
                                    error: OpenIddictConstants.Errors.ServerError,
                                    description: "Cannot access HTTP context.");
                                return;
                            }

                            if (tempUser.OtpCode != otpCode || tempUser.ChallengeId != challengeId || DateTime.UtcNow > tempUser.CreatedAt.AddMinutes(2))
                                context.Reject(
                                    error: OpenIddictConstants.Errors.InvalidClient,
                                    description: "Verification failed"
                                );

                            var identityUser = new IdentityUser()
                            {
                                Id = Guid.NewGuid().ToString(),
                                AccessFailedCount = 0,
                                LockoutEnabled = true,
                                UserName = tempUser?.MobileNumber,
                                PhoneNumber = tempUser?.MobileNumber,
                                PhoneNumberConfirmed = true
                            };

                            var userResult = await userManager.CreateAsync(identityUser);

                            if (!userResult.Succeeded)
                            {
                                context.Reject(
                                    error: OpenIddictConstants.Errors.ServerError,
                                    description: $"Error in creating new user {userResult.Errors}"
                                );

                                return;
                            }

                            user = identityUser;
                        }

                        if (!await signInManager.CanSignInAsync(user!))
                        {
                            context.Reject(
                                error: OpenIddictConstants.Errors.InvalidGrant,
                                description: "User is not allowed to sign in due to unconfirmed account.");
                            return;
                        }


                        var principal = await signInManager.CreateUserPrincipalAsync(user!);

                        if (principal is null)
                        {
                            context.Reject(
                                error: OpenIddictConstants.Errors.ServerError,
                                description: "Could not create principal.");
                            return;
                        }

                        principal.SetScopes(context.Request.GetScopes());
                        principal.SetResources("api");
                        principal.SetClaim(OpenIddictConstants.Claims.Subject, user.Id);

                        foreach (var claim in principal.Claims)
                        {
                            claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken);
                        }

                        context.Principal = principal;

                    });
                });
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });

        services.AddHostedService<ClientSeeder>();


        return services;
    }
}



