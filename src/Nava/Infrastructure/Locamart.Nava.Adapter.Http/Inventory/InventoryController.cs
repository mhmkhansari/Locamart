using Locamart.Nava.Adapter.Http.Inventory.RequestModels;
using Locamart.Nava.Adapter.Http.Product.RequestModels;
using Locamart.Nava.Application.Contracts.UseCases.Inventory;
using Locamart.Nava.Application.Contracts.UseCases.Product.AddComment;
using Locamart.Nava.Application.Contracts.UseCases.Product.GetComments;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Locamart.Nava.Adapter.Http.Inventory;

[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[Route("api/{storeId:guid}/inventories")]
public class InventoryController(IMediator mediator) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> AddInventory(AddInventoryHttpRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddInventoryCommand>();

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}

