using Locamart.Nava.Adapter.Postgresql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace Locamart.Nava.Adapter.Postgresql;

public class LocamartIdentityDbContextFactory : IDesignTimeDbContextFactory<LocamartIdentityDbContext>
{
    public LocamartIdentityDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<LocamartIdentityDbContext>();

        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=locamart_db;Username=postgres;Password=123123123");

        return new LocamartIdentityDbContext(optionsBuilder.Options);
    }
}
