namespace Locamart.Dina.Abstracts;

public interface IDomainEvent;

public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    Task Handle(T domainEvent, CancellationToken cancellationToken);
}
