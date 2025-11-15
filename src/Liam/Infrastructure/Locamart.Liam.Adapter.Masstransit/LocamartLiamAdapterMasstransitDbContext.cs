using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Liam.Adapter.Masstransit;

public class LocamartLiamAdapterMasstransitDbContext : DbContext
{

    public LocamartLiamAdapterMasstransitDbContext(DbContextOptions<LocamartLiamAdapterMasstransitDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LocamartLiamAdapterMasstransitDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddTransactionalOutboxEntities();
    }
}