using CSharpFunctionalExtensions;
using Locamart.Dina;

namespace Locamart.Nava.Domain.Entities.Order.ValueObjects;

public sealed class OrderId : ValueObject<OrderId>, IComparable<OrderId>
{
    public Guid Value { get; }

    private OrderId(Guid value)
    {
        Value = value;
    }

    public static Result<OrderId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Create("order_id_not_valid", "Order Id cannot be empty");

        return new OrderId(value);
    }

    public override string ToString() => Value.ToString();


    public static implicit operator Guid(OrderId orderId) => orderId.Value;
    protected override bool EqualsCore(OrderId other)
    {
        return Value == other.Value;
    }

    protected override int GetHashCodeCore()
    {
        return Value.GetHashCode();
    }

    public int CompareTo(OrderId? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }
}
