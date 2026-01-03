using Locamart.Nava.Application.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Nava.Adapter.Postgresql.QueryServices;

public class InventoryQueryService(LocamartNavaQueryDbContext dbContext) : IInventoryQueryService
{
    public Task<Guid?> GetStoreByInventoryId(Guid inventoryId, CancellationToken cancellationToken)
    {
        return dbContext.Inventories
            .Where(x => x.Id == inventoryId)
            .Select(x => (Guid?)x.StoreId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}

