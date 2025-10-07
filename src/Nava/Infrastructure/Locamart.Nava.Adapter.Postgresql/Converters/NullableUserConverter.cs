using Locamart.Dina.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Locamart.Nava.Adapter.Postgresql.Converters;

public class NullableUserConverter() : ValueConverter<UserId?, Guid?>(id => id != null ? id.Value : (Guid?)null,
    value => value != null ? UserId.Create(value.Value).Value : null);


