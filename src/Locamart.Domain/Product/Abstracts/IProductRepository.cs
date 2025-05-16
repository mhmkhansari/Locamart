using Locamart.Domain.Product.ValueObjects;

namespace Locamart.Domain.Product.Abstracts;

public interface IProductRepository
{
    Task AddAsync(ProductEntity product, CancellationToken cancellationToken);
    Task<ProductEntity?> GetByIdAsync(ProductId id, CancellationToken cancellationToken);
    Task<List<ProductEntity>> GetAllAsync(CancellationToken cancellationToken);
    void Remove(ProductEntity product, CancellationToken cancellationToken);
}