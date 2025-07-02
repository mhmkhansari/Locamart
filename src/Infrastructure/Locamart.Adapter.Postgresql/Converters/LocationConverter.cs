using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Location = Locamart.Shared.ValueObjects.Location;

namespace Locamart.Adapter.Postgresql.Converters;

public class LocationConverter()
    : ValueConverter<Location?, Point?>(
        location => location == null
            ? null
            : new Point(location.Longitude, location.Latitude) { SRID = 4326 },

        point => point == null
            ? null
            : new Location(point.Y, point.X));


