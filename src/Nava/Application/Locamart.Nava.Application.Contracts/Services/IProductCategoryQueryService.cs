using Locamart.Nava.Application.Contracts.Dtos.ProductCategory;

namespace Locamart.Nava.Application.Contracts.Services;

public interface IProductCategoryQueryService
{
    public Task<IEnumerable<ProductCategoryDto>> GetRootChildren(int rootId);
}

