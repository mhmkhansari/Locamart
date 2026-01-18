using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;

namespace Locamart.Nava.Application.Contracts.UseCases.Product.AddProduct;

public class AddProductCommand : ICommand<UnitResult<Error>>
{
    public Guid StoreId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}

