using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;

namespace Locamart.Nava.Adapter.Http.Inventory.RequestModels;

public sealed class AddInventoryHttpRequest : ICommand<UnitResult<Error>>
{
    public Guid ProductId { get; set; }
    public Guid StoreId { get; set; }
    public decimal Price { get; set; }
    public int InitialQuantity { get; set; }
}
