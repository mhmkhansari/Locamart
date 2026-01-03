using Locamart.Adapter.Http.Product.RequestModels;
using Locamart.Dina.Abstracts;
using Locamart.Nava.Adapter.Http.Store.RequestModels;
using Locamart.Nava.Application.Contracts.UseCases.Product.AddProduct;
using Locamart.Nava.Application.Contracts.UseCases.Store;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Locamart.Nava.Adapter.Http.Store;

[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Policy = "StoreAdminPolicy")]
[Route("api/stores")]
public class StoreController(IMediator mediator, ICurrentUser currentUser) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> AddStore(CreateStoreRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddStoreCommand>();

        command.CreatedBy = currentUser.UserId;

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    [HttpPost]
    [Route("/{storeId:guid}/products")]
    public async Task<IActionResult> Create([FromRoute] Guid storeId,
                                            [FromBody] CreateProductHttpRequest request,
                                            CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddProductCommand>();
        command.StoreId = storeId;

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}

