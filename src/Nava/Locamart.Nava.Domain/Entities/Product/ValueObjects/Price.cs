using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.ValueObjects;
using ValueObject = Locamart.Dina.ValueObject;

namespace Locamart.Nava.Domain.Entities.Product.ValueObjects;

public class Price : ValueObject
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

    public Money ToMoney(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidOperationException("Quantity must be greater than zero");

        return Money.Create(Value * quantity, Currency).Value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
        yield return Currency.ToUpperInvariant();
    }
}

