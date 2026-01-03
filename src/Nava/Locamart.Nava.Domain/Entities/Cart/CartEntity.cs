using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Domain.Entities.Cart.Enums;
using Locamart.Nava.Domain.Entities.Cart.ValueObjects;
using Locamart.Nava.Domain.Entities.Inventory.ValueObjects;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Cart;

public sealed class CartEntity : AuditableEntity<CartId>
{
    public UserId OwnerId { get; private set; }
    public StoreId StoreId { get; private set; }
    public List<CartItem> Items { get; private set; } = new();
    public decimal TotalAmount => Items.Sum(i => i.Quantity * i.UnitPrice);
    public int TotalItems => Items.Sum(i => i.Quantity);
    public CartStatus Status { get; private set; }

    private CartEntity(CartId id) : base(id) { }

    public static Result<CartEntity, Error> Create(UserId ownerId, StoreId storeId)
    {
        if (ownerId.Value == Guid.Empty)
            return Error.Create("owner_required", "Owner required!");

        if (storeId.Value == Guid.Empty)
            return Error.Create("store_required", "Store required!");

        var cartIdResult = CartId.Create(Guid.NewGuid());
        if (cartIdResult.IsFailure)
            return cartIdResult.Error;

        return new CartEntity(cartIdResult.Value, ownerId);
    }

    private CartEntity(CartId id, UserId ownerId) : base(id)
    {
        OwnerId = ownerId;
        Status = CartStatus.Active;
    }

    public UnitResult<Error> AddItem(InventoryId inventoryId, int quantity)
    {
        if (Status != CartStatus.Active)
            return Error.Create("invalid_state", "Cart is not active");

        if (quantity <= 0)
            return Error.Create("invalid_quantity", "Invalid cart quantity!");

        var existingItem = Items.FirstOrDefault(i => i.InventoryId == inventoryId);

        if (existingItem != null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            Items.Add(new CartItem(inventoryId, quantity));
        }

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> RemoveItem(InventoryId inventoryId)
    {
        var item = Items.FirstOrDefault(i => i.InventoryId == inventoryId);
        if (item == null)
            return Error.Create("item_not_found", "Cart item not found!");

        Items.Remove(item);
        return UnitResult.Success<Error>();
    }

    public void Clear()
    {
        Items.Clear();
    }

    public UnitResult<Error> Checkout()
    {
        if (Status != CartStatus.Active)
            return Error.Create("invalid_state", "Cart is not active");

        if (!Items.Any())
            return Error.Create("empty_cart", "Cannot checkout an empty cart");

        Status = CartStatus.CheckedOut;

        return UnitResult.Success<Error>();
    }
}

