
using Locamart.Nava.Domain.Entities.ProductCategory;
using Locamart.Nava.Domain.Entities.ProductCategory.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locamart.Nava.Adapter.Postgresql.Configurations;

public sealed class ProductCategoryConfiguration
    : IEntityTypeConfiguration<ProductCategoryEntity>
{
    public void Configure(EntityTypeBuilder<ProductCategoryEntity> builder)
    {

        builder.ToTable("ProductCategories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => ProductCategoryId.Create(value).Value)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(c => c.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(c => c.Status)
            .IsRequired();

        builder.Property(c => c.ParentId)
            .HasConversion(
                id => id != null ? id.Value : (Guid?)null,
                value => value.HasValue
                    ? ProductCategoryId.Create(value.Value).Value
                    : null)
            .HasColumnType("uuid")
            .IsRequired(false);

        builder.HasOne<ProductCategoryEntity>()
            .WithMany()
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.ParentId);

        builder.HasIndex(c => new { c.Status, c.IsDeleted });

        builder.HasIndex(c => new { c.ParentId, c.Name })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

    }
}
