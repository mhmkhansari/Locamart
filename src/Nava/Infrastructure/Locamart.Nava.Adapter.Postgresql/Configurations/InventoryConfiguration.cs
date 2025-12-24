using Locamart.Nava.Adapter.Postgresql.Converters;
using Locamart.Nava.Domain.Entities.Inventory;
using Locamart.Nava.Domain.Entities.Inventory.ValueObjects;
using Locamart.Nava.Domain.Entities.Product;
using Locamart.Nava.Domain.Entities.Product.ValueObjects;
using Locamart.Nava.Domain.Entities.Store;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locamart.Nava.Adapter.Postgresql.Configurations;

public sealed class InventoryConfiguration : IEntityTypeConfiguration<InventoryEntity>
{
    public void Configure(EntityTypeBuilder<InventoryEntity> builder)
    {

        builder.ToTable("Inventories");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasConversion(
                id => id.Value,
                value => InventoryId.Create(value).Value)
            .HasColumnType("uuid")
            .IsRequired();


        builder.Property(i => i.ProductId)
            .HasConversion(
                id => id.Value,
                value => ProductId.Create(value).Value)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(i => i.StoreId)
            .HasConversion(
                id => id.Value,
                value => StoreId.Create(value).Value)
            .HasColumnType("uuid")
            .IsRequired();

        builder.HasOne<ProductEntity>()
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<StoreEntity>()
            .WithMany()
            .HasForeignKey(i => i.StoreId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Property(i => i.AvailableQuantity)
            .IsRequired();

        builder.Property(i => i.ReservedQuantity)
            .IsRequired();


        builder.OwnsOne(i => i.Price, price =>
        {
            price.Property(p => p.Value)
                .HasColumnName("Amount")
                .IsRequired();

            price.Property(p => p.Currency)
                .HasColumnName("Currency")
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

        builder.HasIndex(i => new { i.ProductId, i.StoreId })
            .IsUnique();

        builder.HasIndex(i => i.ProductId);
        builder.HasIndex(i => i.StoreId);

        builder.Ignore(i => i.Atp);
    }
}

