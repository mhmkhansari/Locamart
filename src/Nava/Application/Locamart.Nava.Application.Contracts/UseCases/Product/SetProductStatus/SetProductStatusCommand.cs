using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;

namespace Locamart.Nava.Application.Contracts.UseCases.Product.SetProductStatus;

public class SetProductStatusCommand : ICommand<UnitResult<Error>>
{
    public IEnumerable<Guid> ProductIds { get; init; }
    public short ProductStatus { get; init; }
}

