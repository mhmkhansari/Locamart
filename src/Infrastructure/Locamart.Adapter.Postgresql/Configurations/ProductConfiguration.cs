using Locamart.Adapter.Postgresql.Converters;
using Locamart.Domain.Product;
using Locamart.Domain.Product.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locamart.Adapter.Postgresql.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => ProductId.Create(value))
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(p => p.Title).IsRequired().HasColumnName("Title").HasMaxLength(200);

        builder.Property(p => p.Description).HasColumnName("Description").HasMaxLength(2000);

        builder
            .Property(p => p.Images)
            .HasColumnName("Images")
            .HasConversion(new ImageListConverter())
            .HasColumnType("jsonb")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

