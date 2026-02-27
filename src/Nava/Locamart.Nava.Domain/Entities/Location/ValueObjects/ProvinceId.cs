using CSharpFunctionalExtensions;
using Locamart.Dina;

namespace Locamart.Nava.Domain.Entities.Location.ValueObjects;

public sealed class ProvinceId : ValueObject<ProvinceId>, IComparable<ProvinceId>
{
    public int Value { get; }

    private ProvinceId(int value)
    {
        Value = value;
    }

    internal static ProvinceId From(int value)
        => new(value);

    public static Result<ProvinceId, Error> Create(int value)
    {
        if (value <= 0)
            return Error.Create(
                "province_id_not_valid",
                "Province Id must be a positive integer");

        return new ProvinceId(value);
    }

    public override string ToString() => Value.ToString();

    public static implicit operator int(ProvinceId id) => id.Value;

    protected override bool EqualsCore(ProvinceId other)
        => Value == other.Value;

    protected override int GetHashCodeCore()
        => Value.GetHashCode();

    public int CompareTo(ProvinceId? other)
        => other is null ? 1 : Value.CompareTo(other.Value);
}
