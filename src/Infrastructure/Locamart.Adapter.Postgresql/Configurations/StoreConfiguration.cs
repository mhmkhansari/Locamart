using Locamart.Adapter.Postgresql.Converters;
using Locamart.Domain.Entities.Store;
using Locamart.Domain.Entities.Store.ValueObjects;
using Locamart.Domain.Entities.StoreCategory;
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
                value => StoreId.Create(value).Value)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(p => p.ProfileImage)
            .HasConversion(new ImageConverter())
            .HasColumnName("ProfileImage")
            .HasColumnType("jsonb");

        builder.OwnsOne(x => x.Location, location =>
        {
            location.Property(p => p.Latitude)
                .HasColumnName("Latitude")
                .HasColumnType("double precision");

            location.Property(p => p.Longitude)
                .HasColumnName("Longitude")
                .HasColumnType("double precision");
        });

        builder.OwnsOne(x => x.Identifier, identifier =>
        {
            identifier.Property(p => p.Value)
                .HasColumnName("StoreIdentifier")
                .HasColumnType("varchar(100)");
        });

        builder.HasOne<StoreCategoryEntity>()
            .WithOne()
            .HasForeignKey<StoreEntity>(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

    }
}

