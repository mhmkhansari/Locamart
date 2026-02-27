using Microsoft.EntityFrameworkCore.Storage;

namespace Locamart.Nava.Adapter.Postgresql;

internal sealed class EfCoreTransaction(IDbContextTransaction transaction) : IAsyncDisposable
{
    private bool _committed;

    public async Task CommitAsync(CancellationToken ct = default)
    {
        await transaction.CommitAsync(ct);
        _committed = true;
    }

    public async ValueTask DisposeAsync()
    {
        if (!_committed)
        {
            await transaction.RollbackAsync();
        }

        await transaction.DisposeAsync();
    }
}
