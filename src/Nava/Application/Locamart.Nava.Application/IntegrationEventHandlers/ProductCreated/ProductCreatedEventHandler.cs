using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.IntegrationEvents;
using Locamart.Nava.Application.Contracts.Services;
using Serilog;

namespace Locamart.Nava.Application.IntegrationEventHandlers.ProductCreated;

public class ProductCreatedEventHandler(ISearchService searchService, ILogger logger) : IIntegrationEventHandler<ProductCreatedIntegrationEvent>
{

    public async Task HandleAsync(ProductCreatedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var result = await searchService.IndexProduct(@event);

        if (result.IsFailure)
        {
            logger.Error($"Error while handling ProductCreatedIntegrationEvent: {result.Error}");
        }
    }
}

