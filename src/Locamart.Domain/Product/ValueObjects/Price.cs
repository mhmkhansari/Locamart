using CSharpFunctionalExtensions;
using Locamart.Shared;

namespace Locamart.Domain.Product.ValueObjects;

public sealed class Price : ValueObject<Price>
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Price(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Price amount cannot be negative.");
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency must be provided.", nameof(currency));

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    protected IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public static Price Create(decimal amount, string currency)
    {
        return new Price(amount, currency);
    }

    public Result<Price, Error> Add(Price other)
    {
        if (Currency != other.Currency)
            return Result.Failure<Price, Error>(Error.Create("currency_not_matched", "Input currency is not matched"));

        return new Price(Amount + other.Amount, Currency);
    }

    public Price Subtract(Price other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot subtract prices with different currencies.");

        return new Price(Amount - other.Amount, Currency);
    }

    public override string ToString() => $"{Amount:0.00} {Currency}";

    protected override bool EqualsCore(Price other)
    {
        throw new NotImplementedException();
    }

    protected override int GetHashCodeCore()
    {
        throw new NotImplementedException();
    }
}