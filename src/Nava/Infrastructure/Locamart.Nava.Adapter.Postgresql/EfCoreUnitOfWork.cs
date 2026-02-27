using Locamart.Dina.Abstracts;

namespace Locamart.Nava.Adapter.Postgresql;

public class EfCoreUnitOfWork(LocamartNavaDbContext context) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IAsyncDisposable> BeginTransactionAsync(
        CancellationToken cancellationToken = default
    )
    {
        var transaction = await context.Database
            .BeginTransactionAsync(cancellationToken);

        return new EfCoreTransaction(transaction);
    }
}



