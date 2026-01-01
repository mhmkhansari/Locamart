using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.Dtos.StoreCategory;

namespace Locamart.Nava.Application.Contracts.UseCases.StoreCategory.GetStoreCategories;

public class GetStoreCategoriesQuery : IQuery<Result<IEnumerable<StoreCategoryDto>, Error>>
{
    public string? Name { get; set; }
}

