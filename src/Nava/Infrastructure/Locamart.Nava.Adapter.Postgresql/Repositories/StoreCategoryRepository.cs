using Locamart.Nava.Domain.Entities.StoreCategory;
using Locamart.Nava.Domain.Entities.StoreCategory.Abstracts;
using Locamart.Nava.Domain.Entities.StoreCategory.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Nava.Adapter.Postgresql.Repositories;

public class StoreCategoryRepository(LocamartNavaDbContext dbContext) : IStoreCategoryRepository
{
    public void Add(StoreCategoryEntity entity)
    {
        dbContext.Set<StoreCategoryEntity>().Add(entity);
    }

    public void Update(StoreCategoryEntity entity)
    {
        dbContext.Set<StoreCategoryEntity>().Update(entity);
    }

    public void Delete(StoreCategoryEntity entity)
    {
        dbContext.Set<StoreCategoryEntity>().Remove(entity);
    }

    public async Task<StoreCategoryEntity?> GetById(StoreCategoryId id)
    {
        return await dbContext.Set<StoreCategoryEntity>().Where(x => x.Id == id).SingleOrDefaultAsync();
    }

    public async Task<List<StoreCategoryEntity>> GetAll()
    {
        return await dbContext.Set<StoreCategoryEntity>().ToListAsync();
    }
}

