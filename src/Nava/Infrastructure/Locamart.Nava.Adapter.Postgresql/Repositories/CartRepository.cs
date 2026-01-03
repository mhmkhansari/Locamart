using Locamart.Dina.ValueObjects;
using Locamart.Nava.Domain.Entities.Cart;
using Locamart.Nava.Domain.Entities.Cart.Abstracts;
using Locamart.Nava.Domain.Entities.Cart.Enums;
using Locamart.Nava.Domain.Entities.Cart.ValueObjects;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Nava.Adapter.Postgresql.Repositories;

public class CartRepository(LocamartNavaDbContext dbContext) : ICartRepository
{
    public async Task AddAsync(CartEntity cart, CancellationToken cancellationToken)
    {
        await dbContext.Carts.AddAsync(cart, cancellationToken);
    }

    public async Task<CartEntity?> GetByIdAsync(CartId id, CancellationToken cancellationToken)
    {
        return await dbContext.Carts
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<CartEntity>> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken)
    {
        return await dbContext.Carts
            .AsNoTracking()
            .Where(x => x.CreatedBy == userId &&
                        x.Status == CartStatus.Active)
            .ToListAsync(cancellationToken);
    }

    public async Task<CartEntity?> GetByStoreId(StoreId storeId, CancellationToken cancellationToken)
    {
        return await dbContext.Carts
            .AsNoTracking()
            .Where(x => x.StoreId == storeId &&
                        x.Status == CartStatus.Active)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void Update(CartEntity cart)
    {
        dbContext.Carts.Update(cart);
    }

    public void Delete(CartEntity cart)
    {
        dbContext.Carts.Remove(cart);
    }
}

