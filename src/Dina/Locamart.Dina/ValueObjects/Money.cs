using CSharpFunctionalExtensions;

namespace Locamart.Dina.ValueObjects;

public class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Zero(string currency = "EUR")
        => new Money(0m, currency);

    public static Result<Money, Error> Create(decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
            return Error.Create("invalid_currency", "Currency is required");

        if (amount < 0)
            return Error.Create("invalid_amount", "Money amount cannot be negative");

        return new Money(decimal.Round(amount, 2), currency);
    }

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);

        if (Amount < other.Amount)
            throw new InvalidOperationException("Money cannot go negative");

        return new Money(Amount - other.Amount, Currency);
    }

    public Money Multiply(int multiplier)
    {
        if (multiplier < 0)
            throw new InvalidOperationException("Multiplier cannot be negative");

        return new Money(decimal.Round(Amount * multiplier, 2), Currency);
    }

    private void EnsureSameCurrency(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Currency mismatch");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public static Money Sum(IEnumerable<Money> monies)
    {
        var list = monies.ToList();
        if (!list.Any())
            return Zero();

        var currency = list.First().Currency;

        return list.Aggregate(
            Zero(currency),
            (current, money) => current.Add(money)
        );
    }
}

