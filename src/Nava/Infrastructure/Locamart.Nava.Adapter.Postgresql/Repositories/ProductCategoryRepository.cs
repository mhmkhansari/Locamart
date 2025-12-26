using Locamart.Nava.Domain.Entities.ProductCategory;
using Locamart.Nava.Domain.Entities.ProductCategory.Abstracts;

namespace Locamart.Nava.Adapter.Postgresql.Repositories;

public class ProductCategoryRepository(LocamartNavaDbContext dbContext) : IProductCategoryRepository
{
    public async Task AddAsync(ProductCategoryEntity productCategory, CancellationToken cancellationToken)
    {
        await dbContext.ProductCategories.AddAsync(productCategory, cancellationToken);
    }

    public void Update(ProductCategoryEntity productCategory)
    {
        throw new NotImplementedException();
    }

    public void Remove(ProductCategoryEntity productCategory)
    {
        throw new NotImplementedException();
    }
}

