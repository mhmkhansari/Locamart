using Locamart.Shared.Infrastructure;

namespace Locamart.Adapter.Postgresql;

public class EfCoreUnitOfWork(LocamartDbContext context) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}

