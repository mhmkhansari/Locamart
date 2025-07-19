using Locamart.Nava.Application;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Validation.AspNetCore;

namespace Locamart.Nava.Adapter.Http;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdaptersHttpServices(this IServiceCollection services)
    {

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });

        services.AddOpenIddict()
            .AddValidation(options =>
            {
                options.UseSystemNetHttp();
                options.UseAspNetCore();
            });


        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(ServiceCollectionExtensions).Assembly);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(typeof(IApplicationMarker).Assembly));

        return services;
    }
}