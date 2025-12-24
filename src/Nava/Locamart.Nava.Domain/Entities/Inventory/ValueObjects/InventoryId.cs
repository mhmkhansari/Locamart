using CSharpFunctionalExtensions;
using Locamart.Dina;
using ValueObject = Locamart.Dina.ValueObject;

namespace Locamart.Nava.Domain.Entities.Inventory.ValueObjects;

public sealed class InventoryId : ValueObject, IComparable<InventoryId>
{
    public Guid Value { get; }

    private InventoryId(Guid value)
    {
        Value = value;
    }

    public static Result<InventoryId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Create("product_id_not_valid", "Store Id cannot be empty");

        return new InventoryId(value);
    }

    public override string ToString() => Value.ToString();


    public static implicit operator Guid(InventoryId productId) => productId.Value;

    public int CompareTo(InventoryId? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
    public static InventoryId Empty() => new(Guid.Empty);
}
