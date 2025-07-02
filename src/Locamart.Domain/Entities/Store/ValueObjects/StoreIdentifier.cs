using CSharpFunctionalExtensions;
using Locamart.Shared;
using System.Text.RegularExpressions;

namespace Locamart.Domain.Entities.Store.ValueObjects;

public sealed class StoreIdentifier : ValueObject<StoreIdentifier>
{
    private static readonly Regex Pattern = new(@"^[a-zA-Z][a-zA-Z0-9]*$", RegexOptions.Compiled);

    public string Value { get; }

    private StoreIdentifier(string value)
    {
        Value = value;
    }

    public static Result<StoreIdentifier, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Create("store_identifier_empty", "Store identifier cannot be empty.");

        if (!Pattern.IsMatch(value))
            return Error.Create("store_identifier_invalid",
                "Store identifier must start with a letter and contain only letters and digits.");

        return new StoreIdentifier(value);
    }

    protected override bool EqualsCore(StoreIdentifier other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    protected override int GetHashCodeCore() =>
        Value.ToLowerInvariant().GetHashCode();

    public override string ToString() => Value;


    public static implicit operator string(StoreIdentifier id) => id.Value;
}
