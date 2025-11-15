using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.Services;
using MassTransit;
using MediatR;

namespace Locamart.Nava.Adapter.Masstransit;

public sealed class MassTransitIntegrationEventPublisher(IPublishEndpoint bus)
    : IIntegrationEventPublisher
{
    public Task PublishAsync<T>(T evt, CancellationToken ct = default)
        where T : class, INotification
        => bus.Publish(evt, ct);
}
