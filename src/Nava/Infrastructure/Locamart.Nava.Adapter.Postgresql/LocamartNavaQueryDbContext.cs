using Locamart.Nava.Application.Contracts.Dtos;
using Locamart.Nava.Application.Contracts.Dtos.Cart;
using Locamart.Nava.Application.Contracts.Dtos.Inventory;
using Locamart.Nava.Application.Contracts.Dtos.ProductCategory;
using Locamart.Nava.Application.Contracts.Dtos.Search;
using Locamart.Nava.Application.Contracts.Dtos.StoreCategory;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Nava.Adapter.Postgresql;

public class LocamartNavaQueryDbContext : DbContext
{
    public DbSet<ProductDto> Products => Set<ProductDto>();
    public DbSet<CommentDto> Comments => Set<CommentDto>();
    public DbSet<ProductCategoryDto> ProductCategories => Set<ProductCategoryDto>();
    public DbSet<StoreCategoryDto> StoreCategories => Set<StoreCategoryDto>();
    public DbSet<InventoryDto> Inventories => Set<InventoryDto>();
    public DbSet<CartDto> Carts => Set<CartDto>();

    public LocamartNavaQueryDbContext(DbContextOptions<LocamartNavaQueryDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("nava");

        modelBuilder.Entity<ProductDto>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.ToTable("Products", "nava");
        });


        modelBuilder.Entity<CommentDto>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.ToTable("Comments", "nava");
        });

        modelBuilder.Entity<ProductCategoryDto>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.ToTable("ProductCategory", "nava");

        });

        modelBuilder.Entity<StoreCategoryDto>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.ToTable("StoreCategories", "nava");
        });

        modelBuilder.Entity<InventoryDto>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.ToTable("Inventories", "nava");
        });

        modelBuilder.Entity<CartDto>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.ToTable("Carts", "nava");
            entity.OwnsMany(c => c.Items, a =>
            {

                a.WithOwner()
                    .HasForeignKey("CartId");
            });
        });
    }
}

