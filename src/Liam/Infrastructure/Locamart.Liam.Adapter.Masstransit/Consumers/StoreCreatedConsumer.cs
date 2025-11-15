using Locamart.Dina.Abstracts;
using Locamart.Liam.Application.Contracts.IntegrationEvents;
using MassTransit;
using MassTransit.Mediator;
using Serilog;

namespace Locamart.Liam.Adapter.Masstransit.Consumers;

public class StoreCreatedConsumer(ILogger logger) : IConsumer<StoreCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<StoreCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        logger.Information(
            "Received StoreCreatedIntegrationEvent: for store ({StoreId})",
            message.StoreId);
    }
}

