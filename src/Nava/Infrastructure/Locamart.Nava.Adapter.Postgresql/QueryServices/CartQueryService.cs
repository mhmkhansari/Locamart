using Locamart.Dina.ValueObjects;
using Locamart.Nava.Application.Contracts.Dtos.Cart;
using Locamart.Nava.Application.Contracts.Services;
using Locamart.Nava.Domain.Entities.Cart.Enums;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Nava.Adapter.Postgresql.QueryServices;

public class CartQueryService(LocamartNavaQueryDbContext dbContext) : ICartQueryService
{
    public async Task<UserCartsDto> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken)
    {
        var cartDtos = await dbContext.Carts
       .AsNoTracking()
       .Where(c => c.OwnerId == userId && c.Status == CartStatus.Active)

       .SelectMany(c => c.Items,
                   (c, item) => new { c, item })

       .Join(dbContext.Inventories,
             ci => ci.item.InventoryId,
             inv => inv.Id,
             (ci, inv) => new
             {
                 ci.c.Id,
                 ci.c.OwnerId,
                 ci.c.StoreId,
                 ci.c.Status,
                 Quantity = ci.item.Quantity,
                 Price = inv.Amount
             })

       .GroupBy(x => new
       {
           x.Id,
           x.OwnerId,
           x.StoreId,
           x.Status
       })


       .Select(g => new CartDto
       {
           Id = g.Key.Id,
           OwnerId = g.Key.OwnerId,
           StoreId = g.Key.StoreId,
           Status = g.Key.Status,
           TotalAmount = g.Sum(x => x.Quantity * x.Price)
       })
       .ToListAsync(cancellationToken);

        return new UserCartsDto { UserCarts = cartDtos };
    }
}

