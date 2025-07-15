namespace Locamart.Dina.Abstracts;

public interface IEventBuffer<in TEvent>
    where TEvent : IIntegrationEvent
{
    void Add(TEvent @event);
    Task FlushAsync(CancellationToken cancellationToken = default);
}

