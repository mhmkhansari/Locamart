using Locamart.Nava.Domain.Entities.Cart.Enums;

namespace Locamart.Nava.Application.Contracts.Dtos.Cart;

public class UserCartsDto
{
    public Guid CartId { get; init; }
    public Guid OwnerId { get; init; }
    public Guid StoreId { get; init; }
    public CartStatus Status { get; init; }
    public decimal TotalAmount { get; init; }
}
