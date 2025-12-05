using Locamart.Adapter.Http.Product.RequestModels;
using Locamart.Nava.Adapter.Http.Product.RequestModels;
using Locamart.Nava.Application.Contracts.UseCases.Product.AddComment;
using Locamart.Nava.Application.Contracts.UseCases.Product.AddProduct;
using Locamart.Nava.Application.Contracts.UseCases.Product.GetComments;
using Locamart.Nava.Application.Contracts.UseCases.Product.GetProductsWithinDistance;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Locamart.Nava.Adapter.Http.Product;


[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[Route("api/products")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [Authorize, HttpGet]
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

    [HttpPost("{productId:guid}/comments")]
    public async Task<IActionResult> AddComment(AddCommentHttpRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddCommentCommand>();

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpGet("{productId:guid}/comments")]
    public async Task<IActionResult> GetComments(Guid productId, [FromQuery] Guid? cursor, [FromQuery] int? pageSize, CancellationToken cancellationToken)
    {
        var query = new GetCommentsQuery { ProductId = productId, Cursor = cursor, PageSize = pageSize };

        var result = await mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
