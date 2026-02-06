using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.IntegrationEvents;
using MassTransit;
using ILogger = Serilog.ILogger;

namespace Locamart.Nava.Adapter.Masstransit.Consumers;
public class ProductCreatedConsumer(IIntegrationEventDispatcher dispatcher,
    ILogger logger) : IConsumer<ProductCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedIntegrationEvent> context)
    {
        logger.Information("Consuming ProductCreatedIntegrationEvent from RabbitMQ: {ProductId}", context.Message.ProductId);

        await dispatcher.DispatchAsync(context.Message, context.CancellationToken);
    }
}

