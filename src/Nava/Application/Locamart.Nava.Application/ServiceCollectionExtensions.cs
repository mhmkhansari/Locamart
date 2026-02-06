using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.IntegrationEvents;
using Locamart.Nava.Application.IntegrationEventHandlers.ProductCreated;
using Microsoft.Extensions.DependencyInjection;

namespace Locamart.Nava.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNavaApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IIntegrationEventHandler<ProductCreatedIntegrationEvent>, ProductCreatedEventHandler>();

        return services;
    }
}

