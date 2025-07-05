using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.Services;
using Locamart.Nava.Application.Contracts.UseCases.Product.GetProductsWithinDistance;

namespace Locamart.Nava.Application.UseCases.Product.GetNearbyProducts;

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

