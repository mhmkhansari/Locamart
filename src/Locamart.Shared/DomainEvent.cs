namespace Locamart.Shared;
public abstract class DomainEvent
{
    public DateTimeOffset OccurredOn { get; protected set; } = DateTimeOffset.UtcNow;
}