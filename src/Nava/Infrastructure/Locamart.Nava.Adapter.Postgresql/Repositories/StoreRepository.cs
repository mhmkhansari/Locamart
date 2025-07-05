using Locamart.Adapter.Postgresql;
using Locamart.Nava.Domain.Entities.Store;
using Locamart.Nava.Domain.Entities.Store.Abstracts;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Nava.Adapter.Postgresql.Repositories;

public class StoreRepository(LocamartNavaDbContext dbContext) : IStoreRepository
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

