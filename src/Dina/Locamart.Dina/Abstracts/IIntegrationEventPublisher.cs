namespace Locamart.Dina.Abstracts;

public interface IIntegrationEventDispatcher
{
    Task DispatchAsync<TEvent>(TEvent @event, CancellationToken ct = default) where TEvent : IIntegrationEvent;
}

