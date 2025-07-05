using Locamart.Adapter.Postgresql.Configurations;
using Locamart.Dina;
using Locamart.Nava.Adapter.Postgresql.Configurations;
using Locamart.Nava.Domain.Entities.Product;
using Locamart.Nava.Domain.Entities.Store;
using Locamart.Nava.Domain.Entities.StoreCategory;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Adapter.Postgresql;

public class LocamartNavaDbContext : DbContext
{
    public DbSet<ProductEntity> Products => Set<ProductEntity>();
    public DbSet<StoreEntity> Stores => Set<StoreEntity>();
    public DbSet<StoreCategoryEntity> StoreCategories => Set<StoreCategoryEntity>();

    public LocamartNavaDbContext(DbContextOptions<LocamartNavaDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LocamartNavaDbContext).Assembly);

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
