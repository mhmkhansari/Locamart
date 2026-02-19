using CSharpFunctionalExtensions;
using Locamart.Dina;

namespace Locamart.Nava.Domain.Entities.Order.ValueObjects;

public class OrderPaymentId : ValueObject<OrderPaymentId>, IComparable<OrderPaymentId>
{
    public Guid Value { get; }

    private OrderPaymentId(Guid value)
    {
        Value = value;
    }

    public static Result<OrderPaymentId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Create("order_payment_id_not_valid", "Order Payment Id cannot be empty");

        return new OrderPaymentId(value);
    }

    public override string ToString() => Value.ToString();


    public static implicit operator Guid(OrderPaymentId orderId) => orderId.Value;

    protected override bool EqualsCore(OrderPaymentId other)
    {
        return Value == other.Value;
    }

    protected override int GetHashCodeCore()
    {
        return Value.GetHashCode();
    }

    public int CompareTo(OrderPaymentId? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }
}

