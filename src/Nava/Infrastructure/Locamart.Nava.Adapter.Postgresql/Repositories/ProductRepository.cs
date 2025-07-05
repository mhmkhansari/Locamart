using Locamart.Adapter.Postgresql;
using Locamart.Nava.Domain.Entities.Product;
using Locamart.Nava.Domain.Entities.Product.Abstracts;
using Locamart.Nava.Domain.Entities.Product.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Nava.Adapter.Postgresql.Repositories;

public class ProductRepository(LocamartNavaDbContext context) : IProductRepository
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
