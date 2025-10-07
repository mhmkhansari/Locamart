using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Nava.Application;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Validation.AspNetCore;

namespace Locamart.Nava.Adapter.Http;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdaptersHttpServices(this IServiceCollection services, IConfiguration configuration)
    {
        /* var signingCertPath = configuration["CertificateSettings:SigningCert"];
         var encryptionCertPath = configuration["CertificateSettings:EncryptionCert"];

         if (signingCertPath is null || encryptionCertPath is null)
             throw new InvalidOperationException("Certificate path not found");

         var signingCert = X509CertificateLoader.LoadCertificateFromFile(signingCertPath);

         var encryptionCert = X509CertificateLoader.LoadCertificateFromFile(encryptionCertPath);*/

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });

        services.AddOpenIddict()
            .AddClient(options =>
            {
                options.AddDevelopmentSigningCertificate();
                options.AddDevelopmentEncryptionCertificate();
                options.AllowCustomFlow("otp");
                options.AllowRefreshTokenFlow();

            })
            .AddValidation(options =>
            {
                options.SetIssuer("https://localhost:7046/");

                options.UseSystemNetHttp();
                options.UseAspNetCore();
                /*
                                options.AddEventHandler<OpenIddict.Validation.OpenIddictValidationEvents.ValidateTokenContext>(builder =>
                                {
                                    builder.UseInlineHandler(context =>
                                    {
                                        if (context.Principal is null)
                                        {
                                            Console.WriteLine("No principal could be extracted from the token.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Token was decoded, checking claims:");
                                            foreach (var claim in context.Principal.Claims)
                                                Console.WriteLine($" - {claim.Type}: {claim.Value}");
                                        }

                                        if (context.IsRejected)
                                        {
                                            Console.WriteLine("Validation rejected the token.");
                                            Console.WriteLine($"Error: {context.Error}");
                                            Console.WriteLine($"Description: {context.ErrorDescription}");
                                        }

                                        return default;
                                    });
                                });*/

            });

        services.AddAuthorization();

        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(ServiceCollectionExtensions).Assembly);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(typeof(IApplicationMarker).Assembly));

        services.AddScoped<CurrentUserContext>();
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }
}