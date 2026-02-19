using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Domain.Entities.Order.Enums;
using Locamart.Nava.Domain.Entities.Order.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Order.DomainServices;

public sealed class PaymentDomainService
{
    public Result<OrderPaymentEntity, Error> StartPayment(
        OrderEntity order,
        Money amount,
        PaymentProvider provider,
        PaymentIntentId intentId
    )
    {
        if (order.Status != OrderStatus.PendingPayment)
            return Error.Create("invalid_order_state", "Order is not payable");

        if (order.HasPaymentWithIntent(intentId))
            return Error.Create("duplicate_payment", "Payment intent already processed");

        var paymentResult = OrderPaymentEntity.Create(
            order.Id,
            amount,
            provider,
            intentId
        );

        if (paymentResult.IsFailure)
            return paymentResult.Error;

        order.AttachPayment(paymentResult.Value);

        return paymentResult.Value;
    }

    public UnitResult<Error> CompletePayment(
        OrderEntity order,
        OrderPaymentEntity payment
    )
    {
        if (payment.Status != PaymentStatus.Captured)
            return Error.Create("payment_not_captured", "Payment not completed");

        if (!order.IsFullyPaid())
            return Error.Create("partial_payment", "Order is not fully paid");

        order.MarkPaid();
        return UnitResult.Success<Error>();
    }
}
