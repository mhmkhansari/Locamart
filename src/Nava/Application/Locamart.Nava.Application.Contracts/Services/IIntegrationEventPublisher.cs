using Locamart.Dina.Abstracts;

namespace Locamart.Nava.Application.Contracts.Services;

public interface IIntegrationEventPublisher
{
    public Task PublishAsync<T>(T evt, CancellationToken ct = default)
        where T : class, IIntegrationEvent;
}
