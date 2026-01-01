using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Nava.Application.Contracts.Dtos.StoreCategory;

namespace Locamart.Nava.Application.Contracts.Services;

public interface IStoreCategoryQueryService
{
    Task<Result<IEnumerable<StoreCategoryDto>, Error>> GetStoreCategories(string? name, CancellationToken cancellationToken);
}
