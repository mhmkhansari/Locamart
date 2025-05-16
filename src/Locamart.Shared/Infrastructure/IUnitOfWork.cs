namespace Locamart.Shared.Infrastructure;

public interface IUnitOfWork
{
    public Task CommitAsync(CancellationToken cancellationToken);
}

