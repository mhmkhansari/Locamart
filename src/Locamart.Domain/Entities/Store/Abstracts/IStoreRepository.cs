using Locamart.Domain.Entities.Store;
using Locamart.Domain.Entities.Store.ValueObjects;

namespace Locamart.Domain.Entities.Store.Abstracts;

public interface IStoreRepository
{
    void Add(StoreEntity entity);
    Task Update(StoreEntity entity);
    Task<StoreEntity?> GetById(StoreId id);
}

