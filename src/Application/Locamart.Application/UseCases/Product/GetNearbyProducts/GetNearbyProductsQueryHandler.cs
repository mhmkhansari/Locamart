using CSharpFunctionalExtensions;
using Locamart.Application.Contracts.Services;
using Locamart.Application.Contracts.UseCases.Product.GetProductsWithinDistance;
using Locamart.Shared;
using Locamart.Shared.Abstracts;

namespace Locamart.Application.UseCases.Product.GetNearbyProducts;

public class GetNearbyProductsQueryHandler(ISearchService searchService) : IQueryHandler<GetProductsWithinDistanceQuery, Result<GetProductsWithinDistanceQueryResult, Error>>
{
    public Task<Result<GetProductsWithinDistanceQueryResult, Error>> Handle(GetProductsWithinDistanceQuery request, CancellationToken cancellationToken)
    {
        searchService.GetNearbyProducts(request.Product, request.Distance, request)
    }
}

