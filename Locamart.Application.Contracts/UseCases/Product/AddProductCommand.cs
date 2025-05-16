using CSharpFunctionalExtensions;
using Locamart.Shared;
using Locamart.Shared.Abstracts;

namespace Locamart.Application.Contracts.UseCases.Product;

public class AddProductCommand : ICommand<UnitResult<Error>>
{
    public string Title { get; set; }
    public decimal Price { get; set; }
}

