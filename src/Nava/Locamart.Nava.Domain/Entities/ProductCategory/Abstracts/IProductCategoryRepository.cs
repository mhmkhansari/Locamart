namespace Locamart.Nava.Domain.Entities.ProductCategory.Abstracts;

public interface IProductCategoryRepository
{
    Task AddAsync(ProductCategoryEntity productCategory, CancellationToken cancellationToken);
    void Update(ProductCategoryEntity productCategory);
    void Remove(ProductCategoryEntity productCategory);
}

