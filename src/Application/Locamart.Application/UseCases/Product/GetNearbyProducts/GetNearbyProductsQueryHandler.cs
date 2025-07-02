using CSharpFunctionalExtensions;
using Locamart.Application.Contracts.Services;
using Locamart.Application.Contracts.UseCases.Product.GetProductsWithinDistance;
using Locamart.Shared;
using Locamart.Shared.Abstracts;

namespace Locamart.Application.UseCases.Product.GetNearbyProducts;

public class GetNearbyProductsQueryHandler(ISearchService searchService) : IQueryHandler<GetProductsWithinDistanceQuery, Result<GetProductsWithinDistanceQueryResult, Error>>
{
    public async Task<Result<GetProductsWithinDistanceQueryResult, Error>> Handle(GetProductsWithinDistanceQuery request, CancellationToken cancellationToken)
    {
        var result = await searchService.GetNearbyProducts(request.Product, request.Distance, request.Latitude, request.Longitude);
        if (result.IsFailure)
            return result.Error;

        return new GetProductsWithinDistanceQueryResult()
        {
            Products = result.Value
        };
    }
}

