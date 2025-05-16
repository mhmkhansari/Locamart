using Locamart.Domain.Product;
using Locamart.Domain.Product.ValueObjects;
using Locamart.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Locamart.Adapter.Postgresql;

public class LocamartDbContext : DbContext
{
    public DbSet<ProductEntity> Products => Set<ProductEntity>();

    public LocamartDbContext(DbContextOptions<LocamartDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LocamartDbContext).Assembly);

        modelBuilder.Ignore<DomainEvent>();

        modelBuilder.Entity<ProductEntity>(builder =>
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasConversion(
                    id => id.Value,
                    value => ProductId.Create(value))
                .HasColumnType("uuid")
                .IsRequired();

            builder.HasKey(p => p.Id);


            builder.Property(p => p.Title).IsRequired().HasColumnName("Title").HasMaxLength(200);

            builder.Property(p => p.Description).HasColumnName("Description").HasMaxLength(2000);

            var imagesConverter = new ValueConverter<List<string>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());

            builder
                .Property(typeof(List<string>), "_images")
                .HasColumnName("Images")
                .HasConversion(imagesConverter)
                .HasColumnType("jsonb")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });
    }
}
