using CSharpFunctionalExtensions;

namespace Locamart.Domain.Store.ValueObjects;

public sealed class StoreId : ValueObject<StoreId>
{
    public Guid Value { get; private set; }

    private StoreId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty.", nameof(value));

        Value = value;
    }

    public static StoreId Create(Guid value) => new(value);

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
}
