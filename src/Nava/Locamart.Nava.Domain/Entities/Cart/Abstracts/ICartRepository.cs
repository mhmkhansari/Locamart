using Locamart.Dina.ValueObjects;
using Locamart.Nava.Domain.Entities.Cart.ValueObjects;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Cart.Abstracts;

public interface ICartRepository
{
    Task AddAsync(CartEntity cart, CancellationToken cancellationToken);
    Task<CartEntity?> GetByIdAsync(CartId id, CancellationToken cancellationToken);
    Task<List<CartEntity>> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken);
    Task<CartEntity?> GetByStoreId(StoreId storeId, CancellationToken cancellationToken);
    void Update(CartEntity cart);
    void Delete(CartEntity cart);
}

