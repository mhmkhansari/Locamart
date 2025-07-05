using Locamart.Liam.Application;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Locamart.Liam.Adapter.Http;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLiamAdaptersHttpServices(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(ServiceCollectionExtensions).Assembly);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(typeof(ILiamApplicationMarker).Assembly));

        return services;
    }
}
