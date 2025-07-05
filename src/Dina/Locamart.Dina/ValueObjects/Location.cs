using CSharpFunctionalExtensions;

namespace Locamart.Shared.ValueObjects;

public class Location(double latitude, double longitude) : ValueObject<Location>
{
    public double Latitude { get; init; } = latitude;
    public double Longitude { get; init; } = longitude;

    protected override bool EqualsCore(Location other)
    {
        return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
    }

    protected override int GetHashCodeCore()
    {
        return HashCode.Combine(Latitude, Longitude);
    }
    public override string ToString() => $"({Latitude}, {Longitude})";
}

