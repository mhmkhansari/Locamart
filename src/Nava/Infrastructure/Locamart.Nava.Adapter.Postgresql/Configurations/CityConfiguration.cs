using Locamart.Nava.Adapter.Postgresql.Converters;
using Locamart.Nava.Domain.Entities.Location;
using Locamart.Nava.Domain.Entities.Location.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locamart.Nava.Adapter.Postgresql.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<CityEntity>
{
    public void Configure(EntityTypeBuilder<CityEntity> builder)
    {
        builder.ToTable("Cities");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => CityId.Create(value).Value
            )
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<byte>();

        builder.Property(c => c.ProvinceId)
            .HasConversion(
                id => id.Value,
                value => ProvinceId.Create(value).Value
            )
            .IsRequired();

        builder.HasOne<ProvinceEntity>()
            .WithMany(p => p.Cities)
            .HasForeignKey(c => c.ProvinceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Metadata
            .FindNavigation(nameof(CityEntity.Locales))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

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

        builder.HasIndex(p => p.Name);
    }

}

