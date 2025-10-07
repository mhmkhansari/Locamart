using Locamart.Dina.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Locamart.Nava.Adapter.Postgresql.Converters;

public class UserConverter() : ValueConverter<UserId, Guid>(id => id.Value,
    value => UserId.Create(value).Value);
