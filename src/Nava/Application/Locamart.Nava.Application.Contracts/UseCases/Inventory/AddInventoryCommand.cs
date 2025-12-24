using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;

namespace Locamart.Nava.Application.Contracts.UseCases.Inventory;

public sealed class AddInventoryCommand : ICommand<UnitResult<Error>>
{
    public Guid ProductId { get; set; }
    public Guid StoreId { get; set; }
    public decimal Price { get; set; }
    public int InitialQuantity { get; set; }
}

