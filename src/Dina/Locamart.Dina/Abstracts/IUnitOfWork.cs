namespace Locamart.Dina.Abstracts;

public interface IUnitOfWork
{
    public Task CommitAsync(CancellationToken cancellationToken);
}

