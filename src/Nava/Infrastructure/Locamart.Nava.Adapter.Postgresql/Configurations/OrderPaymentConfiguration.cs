using Locamart.Nava.Adapter.Postgresql.Converters;
using Locamart.Nava.Domain.Entities.Order;
using Locamart.Nava.Domain.Entities.Order.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locamart.Nava.Adapter.Postgresql.Configurations;

public sealed class OrderPaymentConfiguration
    : IEntityTypeConfiguration<OrderPaymentEntity>
{
    public void Configure(EntityTypeBuilder<OrderPaymentEntity> builder)
    {

        builder.ToTable("OrderPayments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => OrderPaymentId.Create(value).Value
            );

        builder.Property(p => p.OrderId)
            .HasColumnName("OrderId")
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => OrderId.Create(value).Value
            );

        builder.OwnsOne(p => p.Amount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("Amount")
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });


        builder.OwnsOne(p => p.Provider, provider =>
        {
            provider.Property(m => m.Code)
                .HasColumnName("ProviderCode")
                .IsRequired();

            provider.Property(m => m.DisplayName)
                .HasColumnName("ProviderName");
        });

        builder.Property(p => p.ProviderReference)
            .HasColumnName("ProviderReference")
            .HasMaxLength(300);

        builder.Property(p => p.IntentId)
            .HasColumnName("IntentId")
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => PaymentIntentId.Create(value).Value
            );

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

        builder.HasIndex(p => p.OrderId);

        builder.HasIndex(p => p.IntentId)
            .IsUnique();
    }
}
