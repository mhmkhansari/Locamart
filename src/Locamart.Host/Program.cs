using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Nava.Adapter.Elasticsearch;
using Locamart.Nava.Adapter.Http;
using Locamart.Nava.Adapter.Http.Middlewares;
using Locamart.Nava.Adapter.Masstransit;
using Locamart.Nava.Adapter.ObjectStorage;
using Locamart.Nava.Adapter.Postgresql;
using Locamart.Nava.Adapter.Redis;
using Locamart.Nava.Application.Contracts.Dtos.User;
using Locamart.Nava.Application.Contracts.Services;
using MassTransit;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using Serilog;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Locamart API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token below (Already prefixed with 'Bearer ')"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = false;
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedPhoneNumber = true;
    })
    .AddEntityFrameworkStores<LocamartIdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<LocamartIdentityDbContext>();
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

                            var tempUser = JsonSerializer.Deserialize<LoginChallengeDto>(jsonTempUser.Value!);

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

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddSingleton(Log.Logger);


//Nava services

builder.Services.AddAdaptersHttpServices(configuration);

builder.Services.AddPostgresqleServices(configuration);

builder.Services.AddObjectStorageServices(configuration);

builder.Services.AddAdapterElasticsearchServices(configuration);

builder.Services.AddAdapterMasstransitServices(configuration);

builder.Services.AddRedisServices(configuration);


builder.Services.AddMassTransit(cfg =>
{
    cfg.AddNavaMasstransit();

    cfg.AddEntityFrameworkOutbox<LocamartNavaDbContext>(o =>
    {
        o.UsePostgres();
        o.UseBusOutbox();
    });

    cfg.UsingInMemory((context, bus) =>
    {
        bus.ConfigureEndpoints(context);
        bus.ConcurrentMessageLimit = Environment.ProcessorCount;
    });
});

builder.Services.AddSingleton<IIntegrationEventDispatcher, IntegrationEventDispatcher>();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await AdapterElasticServiceCollectionExtensions.InitializeAdapterElasticsearchAsync(scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<SetCurrentUserMiddleware>();

app.Run();


