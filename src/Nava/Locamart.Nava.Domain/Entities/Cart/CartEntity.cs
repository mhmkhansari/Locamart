using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Domain.Entities.Cart.Enums;
using Locamart.Nava.Domain.Entities.Cart.ValueObjects;
using Locamart.Nava.Domain.Entities.Product.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Cart;

public sealed class CartEntity : AuditableEntity<CartId>
{
    public UserId OwnerId { get; private set; }
    public List<CartItem> Items { get; private set; } = new();
    public decimal TotalAmount => Items.Sum(i => i.Quantity * i.UnitPrice);
    public CartStatus Status { get; private set; }

    private CartEntity() : base() { }


    public static Result<CartEntity, Error> Create(UserId ownerId)
    {
        if (ownerId.Value == Guid.Empty)
            return Error.Create("owner_required", "Owner required!");

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

    public UnitResult<Error> AddItem(ProductId productId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            return Error.Create("invalid_quantity", "Invalid cart quantity!");

        var existingItem = Items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            Items.Add(new CartItem(productId, quantity, unitPrice));
        }

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> RemoveItem(ProductId productId)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null)
            return Error.Create("item_not_found", "Cart item not found!");

        Items.Remove(item);
        return UnitResult.Success<Error>();
    }

    public void Clear()
    {
        Items.Clear();
    }

    public void Checkout()
    {
        Status = CartStatus.CheckedOut;
    }
}

