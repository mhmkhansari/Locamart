using Locamart.Nava.Adapter.Postgresql.Converters;
using Locamart.Nava.Domain.Entities.Inventory.ValueObjects;
using Locamart.Nava.Domain.Entities.Order;
using Locamart.Nava.Domain.Entities.Order.ValueObjects;
using Locamart.Nava.Domain.Entities.Product.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locamart.Nava.Adapter.Postgresql.Configurations;

public sealed class OrderItemConfiguration
    : IEntityTypeConfiguration<OrderItemEntity>
{
    public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
    {

        builder.ToTable("OrderItems");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasColumnType("uuid")
            .HasConversion(
                id => id.Value,
                value => OrderItemId.Create(value).Value
            );

        builder.Property(i => i.OrderId)
            .HasColumnName("OrderId")
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => OrderId.Create(value).Value
            );

        builder.Property(i => i.ProductId)
            .HasColumnName("ProductId")
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => ProductId.Create(value).Value
            );

        builder.Property(i => i.InventoryId)
            .HasColumnName("InventoryId")
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => InventoryId.Create(value).Value
            );

        builder.Property(i => i.Quantity)
            .HasColumnName("Quantity")
            .IsRequired();


        builder.OwnsOne(i => i.UnitPrice, price =>
        {
            price.Property(p => p.Value)
                .HasColumnName("UnitPriceAmount")
                .HasPrecision(18, 2)
                .IsRequired();

            price.Property(p => p.Currency)
                .HasColumnName("UnitPriceCurrency")
                .HasMaxLength(3)
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

        builder.HasIndex(i => i.OrderId);

        builder.HasIndex(i => new { i.OrderId, i.InventoryId })
            .IsUnique();
    }
}