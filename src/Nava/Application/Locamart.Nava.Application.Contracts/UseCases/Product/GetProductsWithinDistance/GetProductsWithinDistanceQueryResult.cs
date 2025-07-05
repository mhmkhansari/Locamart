using Locamart.Nava.Application.Contracts.Dtos;

namespace Locamart.Nava.Application.Contracts.UseCases.Product.GetProductsWithinDistance;

public class GetProductsWithinDistanceQueryResult
{
    public IReadOnlyCollection<ProductDto> Products { get; set; }
}



