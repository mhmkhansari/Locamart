using Locamart.Application.Contracts.UseCases.Store;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Locamart.Adapter.Http.User;

[ApiController]
[Route("api/user")]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register(CreateStoreRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddStoreCommand>();

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

}

