using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Nava.Application.Contracts.Dtos.StoreCategory;
using Locamart.Nava.Application.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Nava.Adapter.Postgresql.QueryServices;
public class StoreCategoryQueryService(
    LocamartNavaQueryDbContext dbContext
) : IStoreCategoryQueryService
{
    public async Task<Result<IEnumerable<StoreCategoryDto>, Error>> GetStoreCategories(string? name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return await dbContext.StoreCategories
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        try
        {
            var categories = await dbContext.StoreCategories
                .FromSqlRaw("""
                                WITH RECURSIVE matched_categories AS (
                                    -- 1. Categories that match full-text search
                                    SELECT *
                                    FROM nava."StoreCategories"
                                    WHERE to_tsvector('simple', "Name")
                                          @@ plainto_tsquery('simple', {0})
                                ),
                                category_with_parents AS (
                                    -- 2. Start from matched categories
                                    SELECT *
                                    FROM matched_categories

                                    UNION ALL

                                    -- 3. Recursively fetch parents
                                    SELECT sc.*
                                    FROM nava."StoreCategories" sc
                                    INNER JOIN category_with_parents cwp
                                        ON sc."Id" = cwp."ParentId"
                                )
                                SELECT DISTINCT *
                                FROM category_with_parents
                            """, name)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return categories;
        }

        catch (Exception ex)
        {
            return Error.Create("failed_fetch_store_categories", ex.Message);
        }


    }
}

