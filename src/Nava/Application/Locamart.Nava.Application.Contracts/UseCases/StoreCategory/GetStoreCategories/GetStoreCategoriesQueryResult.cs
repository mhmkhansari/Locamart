using Locamart.Nava.Application.Contracts.Dtos.StoreCategory;

namespace Locamart.Nava.Application.Contracts.UseCases.StoreCategory.GetStoreCategories;

public class GetStoreCategoriesQueryResult
{
    public IEnumerable<StoreCategoryDto> StoreCategories { get; set; }
}

