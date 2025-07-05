namespace Locamart.Dina;
public abstract class DomainEvent
{
    public DateTimeOffset OccurredOn { get; protected set; } = DateTimeOffset.UtcNow;
}