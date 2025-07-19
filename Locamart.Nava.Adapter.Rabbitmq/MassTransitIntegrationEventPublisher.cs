using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.Services;
using MassTransit;

namespace Locamart.Nava.Adapter.Rabbitmq;

public class MassTransitIntegrationEventPublisher(IPublishEndpoint publishEndpoint) : IIntegrationEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));

    public Task PublishAsync(IIntegrationEvent integrationEvent)
    {
        if (integrationEvent == null)
            throw new ArgumentNullException(nameof(integrationEvent));

        return _publishEndpoint.Publish(integrationEvent);
    }
}
