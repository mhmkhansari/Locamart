using Locamart.Liam.Application.Contracts.IntegrationEvents;
using Locamart.Liam.Application.Contracts.Services;
using MediatR;
using Serilog;

namespace Locamart.Liam.Application.IntegrationEventHandlers;

public class StoreCreatedEventHandler(IUserStore userStore, ILogger logger) : INotificationHandler<StoreCreatedIntegrationEvent>
{

    public async Task Handle(StoreCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var result = await userStore.AddClaimAsync(notification.OwnerId, "store-admin", notification.StoreId.ToString());

        if (result.IsFailure)
            logger.Error($"Error in processing StoreCreatedIntegrationEvent {result.Error}");
    }
}

