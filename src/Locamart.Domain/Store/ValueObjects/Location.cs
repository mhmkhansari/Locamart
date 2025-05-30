using CSharpFunctionalExtensions;

namespace Locamart.Domain.Store.ValueObjects;

public class Location : ValueObject<Location>
{
    public double Latitude { get; }
    public double Longitude { get; }

    public Location(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    protected override bool EqualsCore(Location other)
    {
        return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
    }

    protected override int GetHashCodeCore()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + Latitude.GetHashCode();
            hash = hash * 23 + Longitude.GetHashCode();
            return hash;
        }
    }
    public override string ToString() => $"({Latitude}, {Longitude})";
}

