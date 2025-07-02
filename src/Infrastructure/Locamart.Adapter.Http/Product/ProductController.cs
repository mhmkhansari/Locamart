using Locamart.Adapter.Http.Product.RequestModels;
using Locamart.Application.Contracts.UseCases.Product.AddProduct;
using Locamart.Application.Contracts.UseCases.Product.GetProductsWithinDistance;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Locamart.Adapter.Http.Product;


[ApiController]
[Route("api/products")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProductsWithinDistance([FromQuery] GetProductsWithinDistanceHttpRequest request,
        CancellationToken cancellationToken)
    {
        var query = request.Adapt<GetProductsWithinDistanceQuery>();

        var result = await mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductHttpRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddProductCommand>();

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
