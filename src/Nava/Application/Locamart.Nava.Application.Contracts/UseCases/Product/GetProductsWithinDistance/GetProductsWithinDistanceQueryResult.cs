using Locamart.Nava.Application.Contracts.Dtos.Search;

namespace Locamart.Nava.Application.Contracts.UseCases.Product.GetProductsWithinDistance;

public class GetProductsWithinDistanceQueryResult
{
    public IReadOnlyCollection<ProductSearchDto> Products { get; set; }
}



