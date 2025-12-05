using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.IntegrationEvents;
using MassTransit;
using Serilog;

namespace Locamart.Liam.Adapter.Masstransit.Consumers;

public class StoreCreatedConsumer(ILogger logger, IIntegrationEventDispatcher dispatcher) : IConsumer<StoreCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<StoreCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        logger.Information(
            "Received StoreCreatedIntegrationEvent: for store ({StoreId})",
            message.StoreId);

        await dispatcher.DispatchAsync<StoreCreatedIntegrationEvent>(context.Message, CancellationToken.None);
    }
}

