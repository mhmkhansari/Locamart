using Locamart.Dina.Abstracts;
using Locamart.Liam.Application.Contracts.Services;
using Locamart.Nava.Application.Contracts.IntegrationEvents;
using Serilog;

namespace Locamart.Liam.Application.IntegrationEventHandlers;

public class StoreCreatedEventHandler(IUserStore userStore, ILogger logger) : IIntegrationEventHandler<StoreCreatedIntegrationEvent>
{
    public async Task HandleAsync(StoreCreatedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var result = await userStore.AddClaimAsync(@event.OwnerId, "store-admin", @event.StoreId.ToString());

        if (result.IsFailure)
            logger.Error($"Error in processing StoreCreatedIntegrationEvent {result.Error}");
    }
}

