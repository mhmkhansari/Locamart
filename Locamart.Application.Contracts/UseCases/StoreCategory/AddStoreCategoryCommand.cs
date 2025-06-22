using CSharpFunctionalExtensions;
using Locamart.Shared;
using Locamart.Shared.Abstracts;

namespace Locamart.Application.Contracts.UseCases.StoreCategory;

public sealed class AddStoreCategoryCommand : ICommand<UnitResult<Error>>
{
    public Guid? ParentId { get; init; }
    public string Name { get; init; }
}

