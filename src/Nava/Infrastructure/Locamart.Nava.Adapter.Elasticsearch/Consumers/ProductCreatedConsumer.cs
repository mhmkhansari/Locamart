using Locamart.Nava.Application.Contracts.IntegrationEvents;
using MassTransit;
using Serilog;

namespace Locamart.Nava.Adapter.Elasticsearch.Consumers;

public class ProductCreatedConsumer(ILogger logger) : IConsumer<ProductCreatedIntegrationEvent>
{
    public Task Consume(ConsumeContext<ProductCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        logger.Information(
            "Received ProductCreatedIntegrationEvent: {ProductId} for store {StoreName} ({StoreId})",
            message.ProductId,
            message.StoreName,
            message.StoreId);

        // TODO: Process the event (e.g., update read models, trigger workflows)

        return Task.CompletedTask;
    }
}
