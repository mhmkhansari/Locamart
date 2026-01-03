namespace Locamart.Nava.Application.Contracts.Services;

public interface IInventoryQueryService
{
    public Task<Guid?> GetStoreByInventoryId(Guid inventoryId, CancellationToken cancellationToken);
}

