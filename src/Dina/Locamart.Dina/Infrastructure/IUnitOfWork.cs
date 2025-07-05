namespace Locamart.Dina.Infrastructure;

public interface IUnitOfWork
{
    public Task CommitAsync(CancellationToken cancellationToken);
}

