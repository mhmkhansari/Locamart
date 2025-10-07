using Locamart.Dina;
using Locamart.Nava.Adapter.Postgresql.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locamart.Nava.Adapter.Postgresql.Extensions;

public static class AuditConfigurationExtensions
{
    public static void ConfigureAuditing<TEntity, TId>(
        this EntityTypeBuilder<TEntity> builder)
        where TEntity : AuditableEntity<TId>
        where TId : notnull
    {
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasConversion(new UserConverter())
            .IsRequired();

        builder.Property(x => x.LastUpdatedAt);

        builder.Property(x => x.UpdatedBy)
            .HasConversion(new NullableUserConverter());
    }
}
