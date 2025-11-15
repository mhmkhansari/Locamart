using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Locamart.Liam.Adapter.Masstransit;

public class LocamartLiamAdapterMasstransitDbContextFactory
    : IDesignTimeDbContextFactory<LocamartLiamAdapterMasstransitDbContext>
{
    public LocamartLiamAdapterMasstransitDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<LocamartLiamAdapterMasstransitDbContext>();

        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=locamart_liam_db;Username=postgres;Password=123123123");

        return new LocamartLiamAdapterMasstransitDbContext(optionsBuilder.Options);
    }
}

