using Locamart.Domain.Entities.StoreCategory;
using Locamart.Domain.Entities.StoreCategory.ValueObjects;

namespace Locamart.Domain.Entities.StoreCategory.Abstracts;

public interface IStoreCategoryRepository
{
    void Add(StoreCategoryEntity entity);
    void Update(StoreCategoryEntity entity);
    void Delete(StoreCategoryEntity entity);
    Task<StoreCategoryEntity?> GetById(StoreCategoryId id);
    Task<List<StoreCategoryEntity>> GetAll();
}

