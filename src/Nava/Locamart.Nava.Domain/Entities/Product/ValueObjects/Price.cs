using CSharpFunctionalExtensions;
using Locamart.Dina;

namespace Locamart.Nava.Domain.Entities.Product.ValueObjects;

public class Price : ValueObject<Price>
{
    public decimal Value { get; init; }
    public string Currency { get; init; }

    public static Result<Price, Error> Create(decimal value, string currency)
    {
        if (value < 0)
            return Error.Create("price_non_negative", "Price cannot be negative");

        return new Price(value, currency);
    }

    private Price(decimal value, string currency)
    {
        Value = value;
        Currency = currency;
    }

    protected override bool EqualsCore(Price other)
    {
        return Value == other.Value && Currency == other.Currency;
    }

    protected override int GetHashCodeCore()
    {
        return HashCode.Combine(Value, Currency);
    }
}

