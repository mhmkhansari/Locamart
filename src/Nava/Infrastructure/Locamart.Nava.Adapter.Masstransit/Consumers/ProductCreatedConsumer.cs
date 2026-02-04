using Locamart.Nava.Application.Contracts.IntegrationEvents;
using MassTransit;
using ILogger = Serilog.ILogger;

namespace Locamart.Nava.Adapter.Masstransit.Consumers;
public class ProductCreatedConsumer(ILogger logger) : IConsumer<ProductCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        logger.Information(
            "Received ProductCreatedIntegrationEvent: {ProductId}",
            message.ProductId);
    }
}

