using Locamart.Nava.Adapter.Postgresql.Converters;
using Locamart.Nava.Domain.Entities.Location;
using Locamart.Nava.Domain.Entities.Location.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locamart.Nava.Adapter.Postgresql.Configurations;

public class LocaleConfiguration : IEntityTypeConfiguration<LocaleEntity>
{
    public void Configure(EntityTypeBuilder<LocaleEntity> builder)
    {
        builder.ToTable("Locales");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasConversion(
                id => id.Value,
                value => LocaleId.Create(value).Value
            )
            .ValueGeneratedOnAdd();

        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.Status)
            .HasConversion<byte>()
            .IsRequired();

        builder.Property(l => l.CityId)
            .HasConversion(
                id => id.Value,
                value => CityId.Create(value).Value
            )
            .IsRequired();

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

        builder.HasOne<CityEntity>()
            .WithMany(c => c.Locales)
            .HasForeignKey(l => l.CityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

