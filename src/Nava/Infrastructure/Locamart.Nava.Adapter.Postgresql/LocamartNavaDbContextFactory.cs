using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Locamart.Nava.Adapter.Postgresql;

public class LocamartNavaDbContextFactory : IDesignTimeDbContextFactory<LocamartNavaDbContext>
{
    public LocamartNavaDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<LocamartNavaDbContext>();

        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=locamart_db;Username=postgres;Password=123123123");

        return new LocamartNavaDbContext(optionsBuilder.Options, new DesignTimeCurrentUser());
    }
}

