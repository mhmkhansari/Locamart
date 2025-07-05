using Locamart.Adapter.Http.Store.RequestModels;
using Locamart.Nava.Application.Contracts.UseCases.Store;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Locamart.Nava.Adapter.Http.Store;

[ApiController]
[Route("api/stores")]
public class StoreController(IMediator mediator) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> AddStore(CreateStoreRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddStoreCommand>();

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}

