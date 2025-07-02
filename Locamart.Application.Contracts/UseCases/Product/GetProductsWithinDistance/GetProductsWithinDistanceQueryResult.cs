using Locamart.Application.Contracts.Dtos;

namespace Locamart.Application.Contracts.UseCases.Product.GetProductsWithinDistance;

public class GetProductsWithinDistanceQueryResult
{
    public List<ProductDto> Products { get; set; }
}



