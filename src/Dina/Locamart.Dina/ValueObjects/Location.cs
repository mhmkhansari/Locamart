using CSharpFunctionalExtensions;

namespace Locamart.Dina.ValueObjects;

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
    public double DistanceTo(Location other)
    {
        var dLat = (other.Latitude - Latitude) * (Math.PI / 180.0);
        var dLon = (other.Longitude - Longitude) * (Math.PI / 180.0);

        var lat1 = Latitude * (Math.PI / 180.0);
        var lat2 = other.Latitude * (Math.PI / 180.0);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        const double earthRadius = 6371000; // meters
        return earthRadius * c;
    }

    public override string ToString() => $"({Latitude}, {Longitude})";
}

