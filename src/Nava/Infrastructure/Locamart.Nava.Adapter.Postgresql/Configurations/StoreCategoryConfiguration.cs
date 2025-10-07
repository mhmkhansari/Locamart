using Locamart.Nava.Adapter.Postgresql.Converters;
using Locamart.Nava.Domain.Entities.StoreCategory;
using Locamart.Nava.Domain.Entities.StoreCategory.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locamart.Nava.Adapter.Postgresql.Configurations;

public class StoreCategoryConfiguration : IEntityTypeConfiguration<StoreCategoryEntity>
{
    public void Configure(EntityTypeBuilder<StoreCategoryEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => StoreCategoryId.Create(value).Value)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(p => p.ParentId)
            .HasConversion(
                id => id.Value,
                value => StoreCategoryId.Create(value).Value)
            .HasColumnType("uuid");

        builder.Property(p => p.CreatedBy)
            .HasConversion(new UserConverter())
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(p => p.UpdatedBy)
            .HasConversion(new NullableUserConverter())
            .HasColumnType("uuid");

        builder.Property(p => p.Name).IsRequired().HasColumnName("Name").HasMaxLength(200);

    }
}

