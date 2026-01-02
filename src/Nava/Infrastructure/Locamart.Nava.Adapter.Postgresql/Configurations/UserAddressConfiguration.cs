using Locamart.Dina.ValueObjects;
using Locamart.Nava.Adapter.Postgresql.Converters;
using Locamart.Nava.Domain.Entities.UserAddress;
using Locamart.Nava.Domain.Entities.UserAddress.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locamart.Nava.Adapter.Postgresql.Configurations;

public sealed class UserAddressEntityConfiguration
    : IEntityTypeConfiguration<UserAddressEntity>
{
    public void Configure(EntityTypeBuilder<UserAddressEntity> builder)
    {
        builder.ToTable("UserAddresses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => UserAddressId.Create(value).Value)
            .HasColumnType("uuid")
            .ValueGeneratedNever();

        builder.Property(x => x.UserId)
            .HasConversion(new UserConverter())
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.AddressText)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.PostalCode)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.ProvinceId);

        builder.Property(x => x.CityId);

        builder.HasIndex(x => new { x.ProvinceId, x.CityId });

        builder.OwnsOne(x => x.Location, location =>
        {
            location.Property(p => p.Latitude)
                .HasColumnName("Latitude")
                .HasColumnType("double precision");

            location.Property(p => p.Longitude)
                .HasColumnName("Longitude")
                .HasColumnType("double precision");
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

        //builder.HasIndex(x => x.UserId);
    }
}
