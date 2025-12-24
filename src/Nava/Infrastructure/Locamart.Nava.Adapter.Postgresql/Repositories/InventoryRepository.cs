using Locamart.Nava.Domain.Entities.Inventory;
using Locamart.Nava.Domain.Entities.Inventory.Abstracts;
using Locamart.Nava.Domain.Entities.Product.ValueObjects;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Nava.Adapter.Postgresql.Repositories;

public class InventoryRepository(LocamartNavaDbContext dbContext) : IInventoryRepository
{
    public async Task AddAsync(InventoryEntity inventory, CancellationToken cancellationToken)
    {
        await dbContext.Inventories.AddAsync(inventory, cancellationToken);
    }

    public async Task<InventoryEntity?> GetByStoreAndProductId(StoreId storeId, ProductId productId, CancellationToken cancellationToken)
    {
        return await dbContext.Inventories
            .Where(x =>
                x.ProductId == productId &&
                x.StoreId == storeId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public void Update(InventoryEntity inventory)
    {
        dbContext.Inventories.Update(inventory);
    }
}

