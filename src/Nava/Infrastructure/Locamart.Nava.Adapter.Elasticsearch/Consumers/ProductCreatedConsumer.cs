using Locamart.Nava.Application.Contracts.IntegrationEvents;
using Locamart.Nava.Application.Contracts.Services;
using MassTransit;
using Serilog;

namespace Locamart.Nava.Adapter.Elasticsearch.Consumers;

public class ProductCreatedConsumer(ILogger logger, ISearchService searchService) : IConsumer<ProductCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        logger.Information(
            "Received ProductCreatedIntegrationEvent: {ProductId} for store {StoreName} ({StoreId})",
            message.ProductId,
            message.StoreName,
            message.StoreId);

        await searchService.IndexProduct(message);
    }
}
