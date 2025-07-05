using Locamart.Nava.Adapter.Postgresql.Converters;
using Locamart.Nava.Domain.Entities.Product;
using Locamart.Nava.Domain.Entities.Product.ValueObjects;
using Locamart.Nava.Domain.Entities.Store;
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
                value => ProductId.Create(value).Value)
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

        builder.HasOne<StoreEntity>()
            .WithMany()
            .HasForeignKey(x => x.StoreId);

        builder.OwnsOne(p => p.Price, price =>
        {
            price.Property(p => p.Value)
                .HasColumnName("Price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            price.Property(p => p.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });
    }
}

