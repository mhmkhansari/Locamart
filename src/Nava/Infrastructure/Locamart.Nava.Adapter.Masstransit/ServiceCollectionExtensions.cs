using Locamart.Nava.Adapter.Masstransit.Consumers;
using Locamart.Nava.Application.Contracts.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Locamart.Nava.Adapter.Masstransit;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdapterMasstransitServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IIntegrationEventPublisher, MassTransitIntegrationEventPublisher>();

        return services;
    }

    public static void AddNavaMasstransit(this IBusRegistrationConfigurator cfg)
    {
        cfg.AddConsumer<ProductCreatedConsumer>();
    }
}

