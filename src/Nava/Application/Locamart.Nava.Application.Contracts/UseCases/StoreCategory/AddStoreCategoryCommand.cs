using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;

namespace Locamart.Nava.Application.Contracts.UseCases.StoreCategory;

public sealed class AddStoreCategoryCommand : ICommand<UnitResult<Error>>
{
    public Guid? ParentId { get; init; }
    public string Name { get; init; }
}

