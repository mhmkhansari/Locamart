using Locamart.Adapter.Http.StoreCategory.RequestModels;
using Locamart.Nava.Application.Contracts.UseCases.StoreCategory;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Locamart.Nava.Adapter.Http.StoreCategory;

[ApiController]
[Route("api/storecategories")]
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

