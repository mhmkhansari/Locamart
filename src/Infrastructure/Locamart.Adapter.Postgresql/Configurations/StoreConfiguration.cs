using Locamart.Adapter.Postgresql.Converters;
using Locamart.Domain.Store;
using Locamart.Domain.Store.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locamart.Adapter.Postgresql.Configurations;

public class StoreConfiguration : IEntityTypeConfiguration<StoreEntity>
{
    public void Configure(EntityTypeBuilder<StoreEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => StoreId.Create(value))
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(p => p.ProfileImage)
            .HasConversion(new ImageConverter())
            .HasColumnName("ProfileImage")
            .HasColumnType("jsonb");

        builder.Property(p => p.Location)
            .HasConversion(new LocationConverter())
            .HasColumnName("Location")
            .HasColumnType("jsonb");

    }
}

