using CSharpFunctionalExtensions;
using Locamart.Dina;
using ValueObject = CSharpFunctionalExtensions.ValueObject;

namespace Locamart.Nava.Domain.Entities.Order.ValueObjects;

public sealed class PaymentIntentId : ValueObject
{
    public Guid Value { get; }

    private PaymentIntentId(Guid value)
    {
        Value = value;
    }

    public static Result<PaymentIntentId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Create("invalid_intent", "Payment intent id is required");

        return new PaymentIntentId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}