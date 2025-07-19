using Locamart.Dina.Abstracts;

namespace Locamart.Nava.Application.Contracts.Services;

public interface IIntegrationEventPublisher
{
    Task PublishAsync(IIntegrationEvent integrationEvent);
}
