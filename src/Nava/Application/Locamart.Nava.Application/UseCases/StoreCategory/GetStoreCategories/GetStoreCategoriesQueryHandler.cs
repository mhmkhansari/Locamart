using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.Dtos.StoreCategory;
using Locamart.Nava.Application.Contracts.Services;
using Locamart.Nava.Application.Contracts.UseCases.StoreCategory.GetStoreCategories;

namespace Locamart.Nava.Application.UseCases.StoreCategory.GetStoreCategories;

public class GetStoreCategoriesQueryHandler(IStoreCategoryQueryService queryService) : IQueryHandler<GetStoreCategoriesQuery, Result<IEnumerable<StoreCategoryDto>, Error>>
{
    public async Task<Result<IEnumerable<StoreCategoryDto>, Error>> Handle(GetStoreCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await queryService.GetStoreCategories(request.Name, cancellationToken);

        return result.IsFailure ? result.Error : result;
    }
}

