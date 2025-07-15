using CSharpFunctionalExtensions;
using Locamart.Nava.Domain.Entities.Cart.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Cart;

public class CartEntity : Entity<CartId>
{
    private CartEntity(CartId id) : base(id)
    {
    }
}

