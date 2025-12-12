using CSharpFunctionalExtensions;
using Locamart.Nava.Domain.Entities.Product.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Cart.ValueObjects;

public class CartItem : ValueObject<CartItem>, IComparable<CartItem>
{
    public ProductId ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    public CartItem(ProductId productId, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public void IncreaseQuantity(int qty)
    {
        Quantity += qty;
    }

    public void DecreaseQuantity(int qty)
    {
        if (qty >= Quantity)
            throw new InvalidOperationException("Cannot decrease more than current quantity");
        Quantity -= qty;
    }

    protected override bool EqualsCore(CartItem other)
    {
        throw new NotImplementedException();
    }

    protected override int GetHashCodeCore()
    {
        throw new NotImplementedException();
    }

    public int CompareTo(CartItem? other)
    {
        throw new NotImplementedException();
    }
}
