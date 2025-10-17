using Locamart.Nava.Adapter.Postgresql.Converters;
using Locamart.Nava.Domain.Entities.Store;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;
using Locamart.Nava.Domain.Entities.StoreCategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locamart.Nava.Adapter.Postgresql.Configurations;

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

        builder.HasOne<StoreCategoryEntity>()
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

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

        builder.Property(p => p.Website)
            .HasConversion(
                uri => uri != null ? uri.ToString() : null,
                str => !string.IsNullOrWhiteSpace(str) ? new Uri(str) : null)
            .HasColumnType("varchar(255)");

        builder.Property(p => p.CreatedBy)
            .HasConversion(new UserConverter())
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(p => p.UpdatedBy)
            .HasConversion(new NullableUserConverter())
            .HasColumnType("uuid");

        builder.Property(p => p.DeletedBy)
            .HasConversion(new NullableUserConverter())
            .HasColumnType("uuid");
    }
}

