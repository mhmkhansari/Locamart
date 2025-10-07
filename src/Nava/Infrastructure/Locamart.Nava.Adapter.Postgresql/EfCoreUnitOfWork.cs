using Locamart.Dina.Infrastructure;

namespace Locamart.Nava.Adapter.Postgresql;

public class EfCoreUnitOfWork(LocamartNavaDbContext context) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}



