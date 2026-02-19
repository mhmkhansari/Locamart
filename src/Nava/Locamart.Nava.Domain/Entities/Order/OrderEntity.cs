using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Domain.Entities.Order.Enums;
using Locamart.Nava.Domain.Entities.Order.ValueObjects;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Order;

public sealed class OrderEntity : AuditableEntity<OrderId>
{
    public UserId UserId { get; private set; }
    public StoreId StoreId { get; private set; }

    public OrderStatus Status { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    public Money Subtotal { get; private set; }
    public Money Total { get; private set; }

    private readonly List<OrderItemEntity> _items = new();
    private readonly List<OrderPaymentEntity> _payments = new();
    public IReadOnlyCollection<OrderItemEntity> Items => _items.AsReadOnly();

    private OrderEntity(OrderId id) : base(id) { }

    private OrderEntity(
        OrderId id,
        UserId userId,
        StoreId storeId
    ) : base(id)
    {
        UserId = userId;
        StoreId = storeId;
        Status = OrderStatus.PendingPayment;
        Subtotal = Money.Zero();
        Total = Money.Zero();
    }

    public static Result<OrderEntity, Error> Create(
        UserId userId,
        StoreId storeId
    )
    {
        var orderIdResult = OrderId.Create(Guid.NewGuid());
        if (orderIdResult.IsFailure)
            return orderIdResult.Error;

        return new OrderEntity(
            orderIdResult.Value,
            userId,
            storeId
        );
    }

    public UnitResult<Error> AddItem(OrderItemEntity item)
    {
        if (Status != OrderStatus.PendingPayment)
            return Error.Create("invalid_state", "Cannot modify a non-pending order");

        _items.Add(item);
        RecalculateTotals();

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> MarkAsPaid()
    {
        if (Status != OrderStatus.PendingPayment)
            return Error.Create("invalid_state", "Order is not payable");

        Status = OrderStatus.Paid;
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Cancel()
    {
        if (Status == OrderStatus.Completed)
            return Error.Create("invalid_state", "Completed order cannot be cancelled");

        Status = OrderStatus.Cancelled;
        return UnitResult.Success<Error>();
    }

    private void RecalculateTotals()
    {
        Subtotal = Money.Sum(_items.Select(i => i.TotalPrice));
        Total = Subtotal; // hooks for discounts, fees later
    }

    public UnitResult<Error> AttachPayment(OrderPaymentEntity payment)
    {
        if (payment.OrderId != Id)
            return Error.Create("order_mismatch", "Payment does not belong to this order");

        if (Status != OrderStatus.PendingPayment)
            return Error.Create("invalid_state", "Order is not awaiting payment");

        _payments.Add(payment);
        return UnitResult.Success<Error>();
    }
    public bool HasPaymentWithIntent(PaymentIntentId intentId)
        => _payments.Any(p => p.IntentId == intentId);

    public Money TotalPaid()
        => Money.Sum(
            _payments
                .Where(p => p.Status == PaymentStatus.Captured)
                .Select(p => p.Amount)
        );

    public bool IsFullyPaid()
        => TotalPaid().Equals(Total);

    public UnitResult<Error> MarkPaid()
    {
        if (_payments.All(p => p.Status != PaymentStatus.Captured))
            return Error.Create("payment_required", "No successful payment found");

        Status = OrderStatus.Paid;
        PaidAt = DateTime.UtcNow;

        return UnitResult.Success<Error>();
    }
}

