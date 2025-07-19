namespace Locamart.Dina;

public abstract class ValueObject : IEquatable<ValueObject>
{
    private int? _cachedHashCode;

    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        return Equals((ValueObject)obj);
    }

    public bool Equals(ValueObject? other)
    {
        if (other is null) return false;
        if (GetType() != other.GetType()) return false;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        if (_cachedHashCode is not null)
            return _cachedHashCode.Value;

        unchecked
        {
            _cachedHashCode = GetEqualityComponents()
                .Aggregate(1, (hash, obj) =>
                    hash * 23 + (obj?.GetHashCode() ?? 0));
            return _cachedHashCode.Value;
        }
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !Equals(left, right);
    }
}
