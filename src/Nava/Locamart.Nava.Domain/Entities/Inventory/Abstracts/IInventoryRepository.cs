using Locamart.Nava.Domain.Entities.Product.ValueObjects;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Inventory.Abstracts;

public interface IInventoryRepository
{
    Task AddAsync(InventoryEntity inventory, CancellationToken cancellationToken);

    Task<InventoryEntity?> GetByStoreAndProductId(StoreId storeId, ProductId productId,
        CancellationToken cancellationToken);
    void Update(InventoryEntity inventory);
}

