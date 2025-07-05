using CSharpFunctionalExtensions;

namespace Locamart.Dina.ValueObjects;

public sealed class Latitude : ValueObject<Latitude>
{
    public double Value { get; }

    private Latitude(double value)
    {
        if (value is < -90 or > 90)
            throw new ArgumentOutOfRangeException(nameof(value), "Latitude must be between -90 and 90.");

        Value = value;
    }

    public static Latitude FromDouble(double value) => new Latitude(value);

    public override string ToString() => Value.ToString("F6");

    protected override bool EqualsCore(Latitude other)
    {
        return Value == other.Value;
    }

    public bool Equals(Latitude other) => Value.Equals(other.Value);

    protected override int GetHashCodeCore()
    {
        return Value.GetHashCode();
    }

    public static implicit operator double(Latitude latitude) => latitude.Value;
}

