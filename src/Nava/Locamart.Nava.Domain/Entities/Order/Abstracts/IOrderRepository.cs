using Locamart.Dina.ValueObjects;
using Locamart.Nava.Domain.Entities.Order.ValueObjects;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Order.Abstracts;

public interface IOrderRepository
{
    Task Add(OrderEntity order, CancellationToken ct);

    Task<OrderEntity?> GetById(
        OrderId orderId,
        CancellationToken ct
    );

    Task<bool> ExistsActiveForUserAndStore(
        UserId userId,
        StoreId storeId,
        CancellationToken ct
    );

    Task Update(OrderEntity order, CancellationToken ct);
}
