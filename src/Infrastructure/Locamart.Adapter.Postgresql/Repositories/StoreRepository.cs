using Locamart.Domain.Store;
using Locamart.Domain.Store.Abstracts;
using Locamart.Domain.Store.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Adapter.Postgresql.Repositories;

public class StoreRepository(LocamartDbContext dbContext) : IStoreRepository
{
    public Task Add(StoreEntity entity)
    {
        dbContext.Add(entity);
        return Task.CompletedTask;
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

