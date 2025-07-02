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

        builder.Property(x => x.Location)
            .HasConversion(new LocationConverter())
            .HasColumnType("geography (point, 4326)");

        builder.HasOne<StoreCategoryEntity>()
            .WithOne()
            .HasForeignKey<StoreEntity>(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

    }
}

