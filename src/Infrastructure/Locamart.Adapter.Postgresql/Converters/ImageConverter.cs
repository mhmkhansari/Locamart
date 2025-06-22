using Locamart.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Locamart.Adapter.Postgresql.Converters;

public class ImageConverter() : ValueConverter<Image?, string>(
    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
    v => JsonSerializer.Deserialize<Image>(v, (JsonSerializerOptions?)null)!);

