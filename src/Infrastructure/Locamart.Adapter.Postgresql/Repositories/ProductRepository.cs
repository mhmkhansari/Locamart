using Locamart.Domain.Entities.Product;
using Locamart.Domain.Entities.Product.Abstracts;
using Locamart.Domain.Entities.Product.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Adapter.Postgresql.Repositories;

public class ProductRepository(LocamartDbContext context) : IProductRepository
{
    public async Task AddAsync(ProductEntity product, CancellationToken cancellationToken)
    {
        await context.AddAsync(product, cancellationToken);
    }

    public async Task<List<ProductEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Products.ToListAsync(cancellationToken);
    }
    public async Task<ProductEntity?> GetByIdAsync(ProductId id, CancellationToken cancellationToken)
    {
        return await context.Products.Where(x => x.Id == id).SingleOrDefaultAsync(cancellationToken);
    }

    public void Remove(ProductEntity entity, CancellationToken cancellationToken)
    {
        context.Products.Remove(entity);
    }
}
