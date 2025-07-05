using Locamart.Nava.Domain.Entities.Store.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Store.Abstracts;

public interface IStoreRepository
{
    void Add(StoreEntity entity);
    Task Update(StoreEntity entity);
    Task<StoreEntity?> GetById(StoreId id);
}

