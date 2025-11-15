using CSharpFunctionalExtensions;
using Locamart.Dina;
using ValueObject = CSharpFunctionalExtensions.ValueObject;

namespace Locamart.Nava.Domain.Entities.Discount.ValueObjects;

public sealed class DiscountId : ValueObject, IComparable<DiscountId>
{
    public Guid Value { get; }

    private DiscountId(Guid value)
    {
        Value = value;
    }

    public static Result<DiscountId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Create("discount_id_not_valid", "Store Id cannot be empty");

        return new DiscountId(value);
    }

    public override string ToString() => Value.ToString();


    public static implicit operator Guid(DiscountId productId) => productId.Value;

    public int CompareTo(DiscountId? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
    public static DiscountId Empty() => new(Guid.Empty);
}