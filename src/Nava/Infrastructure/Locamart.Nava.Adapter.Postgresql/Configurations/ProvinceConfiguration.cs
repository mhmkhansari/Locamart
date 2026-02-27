using Locamart.Nava.Adapter.Postgresql.Converters;
using Locamart.Nava.Domain.Entities.Location;
using Locamart.Nava.Domain.Entities.Location.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locamart.Nava.Adapter.Postgresql.Configurations;

public class ProvinceConfiguration : IEntityTypeConfiguration<ProvinceEntity>
{
    public void Configure(EntityTypeBuilder<ProvinceEntity> builder)
    {
        builder.ToTable("Provinces");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => ProvinceId.Create(value).Value
            )
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<byte>();

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

        builder.HasIndex(p => p.Code)
            .IsUnique();

        builder.HasIndex(p => p.Name);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Metadata
            .FindNavigation(nameof(ProvinceEntity.Cities))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

    }
}

