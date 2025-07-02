using CSharpFunctionalExtensions;
using Locamart.Shared;

namespace Locamart.Domain.Entities.Store.ValueObjects;

public sealed class StoreId : ValueObject<StoreId>, IComparable<StoreId>
{
    public Guid Value { get; init; }

    private StoreId(Guid value)
    {
        Value = value;
    }

    public static Result<StoreId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Create("store_id_not_valid", "Store Id cannot be empty");

        return new StoreId(value);

    }
    public static StoreId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(StoreId productId) => productId.Value;

    protected override bool EqualsCore(StoreId other)
    {
        return Value == other.Value;
    }

    protected override int GetHashCodeCore()
    {
        return Value.GetHashCode();
    }

    public int CompareTo(StoreId? other)
    {
        if (ReferenceEquals(this, other))
            return 0;

        return other is null ? 1 : Value.CompareTo(other.Value);
    }
}
