using CSharpFunctionalExtensions;
using Locamart.Shared;
using Locamart.Shared.Abstracts;

namespace Locamart.Application.Contracts.UseCases.Product.AddProduct;

public class AddProductCommand : ICommand<UnitResult<Error>>
{
    public string Title { get; set; }
    public decimal Price { get; set; }
}

