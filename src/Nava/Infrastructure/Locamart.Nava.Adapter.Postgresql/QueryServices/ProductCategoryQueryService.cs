using Locamart.Nava.Application.Contracts.Dtos.ProductCategory;
using Locamart.Nava.Application.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Nava.Adapter.Postgresql.QueryServices;

public class ProductCategoryQueryService(LocamartNavaQueryDbContext dbContext) : IProductCategoryQueryService
{
    public async Task<IEnumerable<ProductCategoryDto>> GetRootChildren(int rootId)
    {
        var categories = await dbContext.ProductCategories
            .FromSqlRaw("""
                            WITH RECURSIVE category_tree AS (
                                SELECT *, 0 AS depth
                                FROM ProductCategories
                                WHERE id = {0}

                                UNION ALL

                                SELECT c.*, ct.depth + 1
                                FROM product_categories c
                                JOIN category_tree ct ON c.parent_id = ct.id
                            )
                            SELECT * FROM category_tree
                        """, rootId)
            .AsNoTracking()
            .ToListAsync();

        return categories;
    }
}

