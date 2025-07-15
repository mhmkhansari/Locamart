using CSharpFunctionalExtensions;
using Locamart.Dina;

namespace Locamart.Nava.Domain.Entities.Cart.ValueObjects;

public sealed class CartId : ValueObject<CartId>, IComparable<CartId>
{
    public Guid Value { get; }

    private CartId(Guid value)
    {
        Value = value;
    }

    public static Result<CartId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Create("cart_id_not_valid", "Store Id cannot be empty");

        return new CartId(value);
    }

    public override string ToString() => Value.ToString();


    public static implicit operator Guid(CartId cartId) => cartId.Value;
    protected override bool EqualsCore(CartId other)
    {
        return Value == other.Value;
    }

    protected override int GetHashCodeCore()
    {
        return Value.GetHashCode();
    }

    public int CompareTo(CartId? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }
}
