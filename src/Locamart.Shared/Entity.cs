using CSharpFunctionalExtensions;

namespace Locamart.Shared;

public abstract class Entity<TId>(TId id)
    where TId : ValueObject<TId>
{
    private List<DomainEvent>? _domainEvents;

    public TId Id { get; private set; } = id ?? throw new ArgumentNullException(nameof(id), "Entity ID cannot be null.");

    public IReadOnlyCollection<DomainEvent> DomainEvents =>
        _domainEvents != null ? _domainEvents.AsReadOnly() : Array.Empty<DomainEvent>();

    public void Raise(DomainEvent eventItem)
    {
        _domainEvents ??= new List<DomainEvent>();
        _domainEvents.Add(eventItem);
    }

    public void ClearDomainEvents() => _domainEvents?.Clear();

    #region Equality Members

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (obj is null || obj.GetType() != GetType())
            return false;

        var other = (Entity<TId>)obj;

        return Id == other.Id;
    }

    public override int GetHashCode() => Id?.GetHashCode() ?? 0;

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right) => !(left == right);

    #endregion
}