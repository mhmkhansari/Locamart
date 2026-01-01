using Locamart.Adapter.Http.StoreCategory.RequestModels;
using Locamart.Nava.Application.Contracts.UseCases.StoreCategory.AddStoreCategory;
using Locamart.Nava.Application.Contracts.UseCases.StoreCategory.GetStoreCategories;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Locamart.Nava.Adapter.Http.StoreCategory;

[ApiController]
[Route("api/storecategories")]

public class StoreCategoryController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public async Task<IActionResult> AddStoreCategory(CreateStoreCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddStoreCategoryCommand>();

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetStoreCategories([FromQuery] string? name,
        CancellationToken cancellationToken)
    {
        var query = new GetStoreCategoriesQuery() { Name = name };

        var result = await mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

}

