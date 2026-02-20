using Locamart.Dina.ValueObjects;
using Locamart.Nava.Domain.Entities.Order;
using Locamart.Nava.Domain.Entities.Order.Abstracts;
using Locamart.Nava.Domain.Entities.Order.ValueObjects;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Locamart.Nava.Adapter.Postgresql.Repositories;

public sealed class OrderRepository(LocamartNavaDbContext db) : IOrderRepository
{
    public async Task Add(OrderEntity order, CancellationToken ct)
    {
        await db.Orders.AddAsync(order, ct);
    }

    public async Task<OrderEntity?> GetById(
        OrderId orderId,
        CancellationToken ct
    )
    {
        return await db.Orders
            .Include(o => o.Items)
            .SingleOrDefaultAsync(
                o => o.Id == orderId,
                ct
            );
    }

    public async Task<bool> ExistsActiveForUserAndStore(
        UserId userId,
        StoreId storeId,
        CancellationToken ct
    )
    {
        const string sql = """
                               SELECT 1
                               FROM orders
                               WHERE user_id = @userId
                                 AND store_id = @storeId
                                 AND status IN ('Created', 'ReadyForPayment', 'PendingPayment')
                               LIMIT 1
                               FOR UPDATE;
                           """;

        var userIdParam = new NpgsqlParameter("userId", userId.Value);
        var storeIdParam = new NpgsqlParameter("storeId", storeId.Value);

        var result = await db.Database
            .SqlQueryRaw<int>(sql, userIdParam, storeIdParam)
            .FirstOrDefaultAsync(ct);

        return result == 1;
    }

    public Task Update(OrderEntity order, CancellationToken ct)
    {
        db.Orders.Update(order);
        return Task.CompletedTask;
    }
}
