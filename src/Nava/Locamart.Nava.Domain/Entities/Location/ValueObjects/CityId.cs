using CSharpFunctionalExtensions;
using Locamart.Dina;

namespace Locamart.Nava.Domain.Entities.Location.ValueObjects;

public sealed class CityId : ValueObject<CityId>, IComparable<CityId>
{
    public int Value { get; }

    private CityId(int value)
    {
        Value = value;
    }

    internal static CityId From(int value)
        => new(value);

    public static Result<CityId, Error> Create(int value)
    {
        if (value <= 0)
            return Error.Create(
                "city_id_not_valid",
                "City Id must be a positive integer");

        return new CityId(value);
    }

    public override string ToString() => Value.ToString();

    public static implicit operator int(CityId id) => id.Value;

    protected override bool EqualsCore(CityId other)
        => Value == other.Value;

    protected override int GetHashCodeCore()
        => Value.GetHashCode();

    public int CompareTo(CityId? other)
        => other is null ? 1 : Value.CompareTo(other.Value);
}
