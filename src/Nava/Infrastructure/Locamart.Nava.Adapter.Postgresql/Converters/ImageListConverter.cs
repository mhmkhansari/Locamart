using Locamart.Dina.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Locamart.Nava.Adapter.Postgresql.Converters;

public class ImageListConverter() : ValueConverter<List<Image>, string>(
    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
    v => JsonSerializer.Deserialize<List<Image>>(v, (JsonSerializerOptions?)null) ?? new List<Image>());
