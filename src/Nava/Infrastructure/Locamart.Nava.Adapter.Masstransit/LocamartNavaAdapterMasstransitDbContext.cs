using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Nava.Adapter.Masstransit;

public class LocamartNavaAdapterMasstransitDbContext : DbContext
{
    public LocamartNavaAdapterMasstransitDbContext(DbContextOptions<LocamartNavaAdapterMasstransitDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddTransactionalOutboxEntities();
    }
}

