using Locamart.Adapter.Http.Product.RequestModels;
using Locamart.Application.Contracts.UseCases.Product;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Locamart.Adapter.Http.Product;


[ApiController]
[Route("api/products")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll() => Ok(new[] { "Sample Product" });

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductHttpRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddProductCommand>();

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
