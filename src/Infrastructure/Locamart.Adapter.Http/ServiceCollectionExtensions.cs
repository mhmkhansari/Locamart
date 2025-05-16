using Locamart.Application;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Locamart.Adapter.Http;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdaptersHttpServices(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(ServiceCollectionExtensions).Assembly);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(typeof(IApplicationMarker).Assembly));

        return services;
    }
}
