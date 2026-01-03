using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;

namespace Locamart.Nava.Application.Contracts.UseCases.Cart.AddToCart;

public class AddToCartCommand : ICommand<UnitResult<Error>>
{
    public Guid UserId { get; set; }
    public Guid InventoryId { get; set; }
    public int Quantity { get; set; }
}

