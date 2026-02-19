namespace Locamart.Nava.Domain.Entities.Order.Enums;

public enum OrderStatus
{
    PendingPayment = 1,
    Paid = 2,
    Preparing = 3,
    ReadyForPickup = 4,
    Completed = 5,
    Cancelled = 6,
    PaymentFailed = 7,
    Rejected = 8
}