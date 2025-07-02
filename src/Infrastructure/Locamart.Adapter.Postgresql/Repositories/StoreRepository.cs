using Locamart.Domain.Entities.Store;
using Locamart.Domain.Entities.Store.Abstracts;
using Locamart.Domain.Entities.Store.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Adapter.Postgresql.Repositories;

public class StoreRepository(LocamartDbContext dbContext) : IStoreRepository
{
    public void Add(StoreEntity entity)
    {
        dbContext.Add(entity);
    }

    public Task Update(StoreEntity entity)
    {
        dbContext.Update(entity);
        return Task.CompletedTask;
    }

    public async Task<StoreEntity?> GetById(StoreId id)
    {
        return await dbContext.Set<StoreEntity>().Where(x => x.Id == id).SingleOrDefaultAsync();
    }
}

