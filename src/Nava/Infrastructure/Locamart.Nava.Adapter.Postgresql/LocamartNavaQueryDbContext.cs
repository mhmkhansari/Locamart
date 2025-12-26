using Locamart.Nava.Application.Contracts.Dtos;
using Locamart.Nava.Application.Contracts.Dtos.ProductCategory;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Nava.Adapter.Postgresql;

public class LocamartNavaQueryDbContext : DbContext
{
    public DbSet<ProductDto> Products => Set<ProductDto>();
    public DbSet<CommentDto> Comments => Set<CommentDto>();
    public DbSet<ProductCategoryDto> ProductCategories => Set<ProductCategoryDto>();

    public LocamartNavaQueryDbContext(DbContextOptions<LocamartNavaQueryDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductDto>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.ToTable("Products");
        });


        modelBuilder.Entity<CommentDto>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.ToTable("Comments");
        });

        modelBuilder.Entity<ProductCategoryDto>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.ToTable("ProductCategory");

        });
    }
}

