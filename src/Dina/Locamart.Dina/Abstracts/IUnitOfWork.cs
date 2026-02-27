namespace Locamart.Dina.Abstracts;

public interface IUnitOfWork
{
    public Task CommitAsync(CancellationToken cancellationToken);
    Task<IAsyncDisposable> BeginTransactionAsync(
        CancellationToken cancellationToken = default
    );
}

