using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Locamart.Nava.Adapter.Masstransit;

public class LocamartNavaAdapterMasstransitDbContextFactory : IDesignTimeDbContextFactory<LocamartNavaAdapterMasstransitDbContext>
{
    public LocamartNavaAdapterMasstransitDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<LocamartNavaAdapterMasstransitDbContext>();

        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=locamart_db;Username=postgres;Password=123123123");

        return new LocamartNavaAdapterMasstransitDbContext(optionsBuilder.Options);
    }
}

