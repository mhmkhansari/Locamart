using Locamart.Liam.Application.Contracts.Dtos.User;
using Locamart.Liam.Application.Contracts.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using System.Text.Json;

namespace Locamart.Liam.Adapter.Postgresql;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLiamPostgresServices(this IServiceCollection services, IConfiguration configuration)
    {
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
                    .AllowCustomFlow("otp")
                    .AllowRefreshTokenFlow()
                    .RegisterScopes("api")
                    .AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate()
                    .UseAspNetCore()
                    .DisableTransportSecurityRequirement();

                options.AddEventHandler<OpenIddictServerEvents.HandleTokenRequestContext>(builder =>
                {
                    builder.UseInlineHandler(async context =>
                    {
                        if (context.Request.GrantType != "otp")
                            return;

                        var userId = context.Request.GetParameter("username")?.ToString();
                        var otpCode = context.Request.GetParameter("otp_code")?.ToString();
                        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(otpCode))
                        {
                            context.Reject(
                                error: OpenIddictConstants.Errors.InvalidRequest,
                                description: "The user_id and otp_code parameters are required.");
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

                        var mobileNumber = context.Request.Username;

                        var jsonTempUser = await cacheService.GetAsync($"temp_user:{userId}");

                        if (jsonTempUser.IsFailure)
                            context.Reject(
                                error: OpenIddictConstants.Errors.InvalidClient,
                                description: jsonTempUser.Error.ToString()
                            );

                        if (jsonTempUser.Value is null)
                            context.Reject(
                                error: OpenIddictConstants.Errors.InvalidClient,
                                description: jsonTempUser.Error.ToString()
                            );

                        var tempUser = JsonSerializer.Deserialize<TempUserDto>(jsonTempUser.Value!);

                        if (tempUser?.OtpCode != otpCode || DateTime.UtcNow > tempUser.CreatedAt.AddMinutes(2))
                            context.Reject(
                                error: OpenIddictConstants.Errors.InvalidClient,
                                description: "Otp expired"
                            );

                        var user = await userManager.FindByNameAsync(mobileNumber!);
                        if (user == null)
                        {
                            var identityUser = new IdentityUser()
                            {
                                Id = Guid.NewGuid().ToString(),
                                AccessFailedCount = 0,
                                LockoutEnabled = true,
                                UserName = tempUser.MobileNumber,
                                PhoneNumber = tempUser.MobileNumber
                            };

                            var userResult = await userManager.CreateAsync(identityUser);

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
                        principal.SetClaim(OpenIddictConstants.Claims.Subject, user.UserName);

                        foreach (var claim in principal.Claims)
                        {
                            claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken);
                        }

                        var signInContext = new OpenIddictServerEvents.ProcessSignInContext(context.Transaction)
                        {
                            Principal = principal,
                            Request = context.Request
                        };

                        /*var dispatcher = httpContext.RequestServices.GetRequiredService<IOpenIddictServerDispatcher>();
                        await dispatcher.DispatchAsync(signInContext);

                        if (signInContext.IsRejected)
                        {
                            context.Reject(
                                error: signInContext.Error,
                                description: signInContext.ErrorDescription);
                            return;
                        }*/

                        context.Principal = principal;

                        // context.HandleRequest();
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



