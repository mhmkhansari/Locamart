using Locamart.Nava.Domain.Entities.Cart.Enums;

namespace Locamart.Nava.Application.Contracts.Dtos.Cart;

public class UserCartsDto
{
    public IEnumerable<CartDto> UserCarts { get; set; }
}

public class CartDto
{
    public Guid Id { get; init; }
    public Guid OwnerId { get; init; }
    public Guid StoreId { get; init; }
    public CartStatus Status { get; init; }
    public decimal TotalAmount { get; init; }
    public IEnumerable<CartItemDto> Items { get; init; }
}

public class CartItemDto
{
    public Guid InventoryId { get; init; }
    public int Quantity { get; init; }
}
