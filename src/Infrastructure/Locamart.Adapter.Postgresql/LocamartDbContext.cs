using Locamart.Adapter.Postgresql.Configurations;
using Locamart.Domain.Entities.Product;
using Locamart.Domain.Entities.Store;
using Locamart.Domain.Entities.StoreCategory;
using Locamart.Shared;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Adapter.Postgresql;

public class LocamartDbContext : DbContext
{
    public DbSet<ProductEntity> Products => Set<ProductEntity>();
    public DbSet<StoreEntity> Stores => Set<StoreEntity>();
    public DbSet<StoreCategoryEntity> StoreCategories => Set<StoreCategoryEntity>();

    public LocamartDbContext(DbContextOptions<LocamartDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LocamartDbContext).Assembly);

        modelBuilder.Ignore<DomainEvent>();

        modelBuilder.ApplyConfiguration(new ProductConfiguration());

        modelBuilder.ApplyConfiguration(new StoreConfiguration());

        modelBuilder.ApplyConfiguration(new StoreCategoryConfiguration());

    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}
