using Locamart.Dina.ValueObjects;
using Locamart.Nava.Adapter.Postgresql.Converters;
using Locamart.Nava.Domain.Entities.Cart;
using Locamart.Nava.Domain.Entities.Cart.ValueObjects;
using Locamart.Nava.Domain.Entities.Inventory.ValueObjects;
using Locamart.Nava.Domain.Entities.Product.ValueObjects;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;
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
            .HasConversion(new UserConverter())
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(c => c.StoreId)
            .HasConversion(
                storeId => storeId.Value,
                value => StoreId.Create(value).Value)
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

            items.HasKey("CartId", "InventoryId");

            items.Property(i => i.InventoryId)
                .HasConversion(
                    iid => iid.Value,
                    value => InventoryId.Create(value).Value)
                .HasColumnType("uuid")
                .IsRequired();

            items.Property(i => i.Quantity)
                .IsRequired();
        });

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



