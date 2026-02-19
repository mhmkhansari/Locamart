using CSharpFunctionalExtensions;
using Locamart.Dina;
using ValueObject = CSharpFunctionalExtensions.ValueObject;

namespace Locamart.Nava.Domain.Entities.Order.ValueObjects;

public sealed class PaymentProvider : ValueObject
{
    public string Code { get; }
    public string DisplayName { get; }

    private PaymentProvider(string code, string displayName)
    {
        Code = code;
        DisplayName = displayName;
    }

    public static Result<PaymentProvider, Error> Create(
        string code,
        string displayName
    )
    {
        if (string.IsNullOrWhiteSpace(code))
            return Error.Create("invalid_provider_code", "Provider code is required");

        if (string.IsNullOrWhiteSpace(displayName))
            return Error.Create("invalid_provider_name", "Provider display name is required");

        return new PaymentProvider(
            code.Trim().ToUpperInvariant(),
            displayName.Trim()
        );
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }

    public override string ToString() => Code;
}