using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.IntegrationEvents;
using MassTransit;
using MediatR;
using ILogger = Serilog.ILogger;

namespace Locamart.Nava.Adapter.Masstransit.Consumers;
public class ProductCreatedConsumer(ILogger logger, INotificationHandler<ProductCreatedIntegrationEvent> handler) : IConsumer<ProductCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        logger.Information(
            "Received ProductCreatedIntegrationEvent: {ProductId} for store {StoreName} ({StoreId})",
            message.ProductId,
            message.StoreName,
            message.StoreId);

        await handler.Handle(message, CancellationToken.None);
    }
}

