using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Domain.Entities.Order.Enums;
using Locamart.Nava.Domain.Entities.Order.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Order;

public sealed class OrderPaymentEntity : CSharpFunctionalExtensions.Entity<OrderPaymentId>
{
    public OrderId OrderId { get; private set; }

    public Money Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public PaymentIntentId IntentId { get; private set; }

    public PaymentProvider Provider { get; private set; }
    public string ProviderReference { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private OrderPaymentEntity(OrderPaymentId id) : base(id)
    {
    }

    private OrderPaymentEntity(
        OrderPaymentId id,
        OrderId orderId,
        Money amount,
        PaymentProvider provider,
        PaymentIntentId intentId
    ) : base(id)
    {
        OrderId = orderId;
        Amount = amount;
        Provider = provider;
        IntentId = intentId;
        Status = PaymentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<OrderPaymentEntity, Error> Create(
        OrderId orderId,
        Money amount,
        PaymentProvider provider,
        PaymentIntentId intentId
    )
    {
        if (amount.Amount <= 0)
            return Error.Create("invalid_amount", "Payment amount must be greater than zero");

        var paymentIdResult = OrderPaymentId.Create(Guid.NewGuid());
        if (paymentIdResult.IsFailure)
            return paymentIdResult.Error;

        return new OrderPaymentEntity(
            paymentIdResult.Value,
            orderId,
            amount,
            provider,
            intentId
        );
    }
}
