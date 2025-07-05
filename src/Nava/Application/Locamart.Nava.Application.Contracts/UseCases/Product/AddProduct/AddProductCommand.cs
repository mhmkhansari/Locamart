using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;

namespace Locamart.Nava.Application.Contracts.UseCases.Product.AddProduct;

public class AddProductCommand : ICommand<UnitResult<Error>>
{
    public string Title { get; set; }
    public decimal Price { get; set; }
}

