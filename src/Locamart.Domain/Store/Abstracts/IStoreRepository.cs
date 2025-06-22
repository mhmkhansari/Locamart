using Locamart.Domain.Store.ValueObjects;

namespace Locamart.Domain.Store.Abstracts;

public interface IStoreRepository
{
    Task Add(StoreEntity entity);
    Task Update(StoreEntity entity);
    Task<StoreEntity?> GetById(StoreId id);
}

