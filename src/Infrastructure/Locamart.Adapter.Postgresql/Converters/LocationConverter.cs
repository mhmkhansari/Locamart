using Locamart.Domain.Store.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Locamart.Adapter.Postgresql.Converters;

public class LocationConverter() : ValueConverter<Location?, string>(
    location => JsonSerializer.Serialize(location, (JsonSerializerOptions?)null),
    json => JsonSerializer.Deserialize<Location>(json, (JsonSerializerOptions?)null)!);
