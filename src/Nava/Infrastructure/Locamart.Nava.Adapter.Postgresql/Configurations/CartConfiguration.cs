using Locamart.Dina.ValueObjects;
using Locamart.Nava.Adapter.Postgresql.Converters;
using Locamart.Nava.Domain.Entities.Cart;
using Locamart.Nava.Domain.Entities.Cart.ValueObjects;
using Locamart.Nava.Domain.Entities.Product.ValueObjects;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;
using Locamart.Nava.Domain.Entities.StoreCategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locamart.Nava.Adapter.Postgresql.Configurations;
public class CartConfiguration : IEntityTypeConfiguration<CartEntity>
{

    public void Configure(EntityTypeBuilder<CartEntity> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => CartId.Create(value).Value)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(c => c.OwnerId)
            .HasConversion(
                ownerId => ownerId.Value,
                value => UserId.Create(value).Value)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.OwnsMany(c => c.Items, items =>
        {
            items.ToTable("CartItems");

            items.WithOwner().HasForeignKey("CartId");

            items.HasKey("CartId", "ProductId");

            items.Property(i => i.ProductId)
                .HasConversion(
                    pid => pid.Value,
                    value => ProductId.Create(value).Value)
                .HasColumnType("uuid")
                .IsRequired();

            items.Property(i => i.Quantity)
                .IsRequired();

            items.Property(i => i.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        });

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.LastUpdatedAt)
            .IsRequired(false);
    }
}



