using Locamart.Adapter.Http.StoreCategory.RequestModels;
using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.UseCases.StoreCategory;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Locamart.Nava.Adapter.Http.StoreCategory;

[ApiController]
[Route("api/storecategories")]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
public class StoreCategoryController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddStoreCategory(CreateStoreCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddStoreCategoryCommand>();

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

}

