using CSharpFunctionalExtensions;
using Locamart.Dina;

namespace Locamart.Nava.Domain.Entities.Order.ValueObjects;

public sealed class OrderItemId : ValueObject<OrderItemId>, IComparable<OrderItemId>
{
    public Guid Value { get; }

    private OrderItemId(Guid value)
    {
        Value = value;
    }

    public static Result<OrderItemId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Create("order_item_id_not_valid", "Order Item Id cannot be empty");

        return new OrderItemId(value);
    }

    public override string ToString() => Value.ToString();


    public static implicit operator Guid(OrderItemId orderItemId) => orderItemId.Value;
    protected override bool EqualsCore(OrderItemId other)
    {
        return Value == other.Value;
    }

    protected override int GetHashCodeCore()
    {
        return Value.GetHashCode();
    }

    public int CompareTo(OrderItemId? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }
}