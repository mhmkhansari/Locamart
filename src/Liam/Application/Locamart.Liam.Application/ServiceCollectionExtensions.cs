using Locamart.Dina.Abstracts;
using Locamart.Liam.Application.IntegrationEventHandlers;
using Locamart.Nava.Application.Contracts.IntegrationEvents;
using Microsoft.Extensions.DependencyInjection;

namespace Locamart.Liam.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLiamApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IIntegrationEventHandler<StoreCreatedIntegrationEvent>, StoreCreatedEventHandler>();

        return services;
    }
}

