using Locamart.Dina.Abstracts;
using Locamart.Nava.Adapter.Http.Cart.RequestModels;
using Locamart.Nava.Application.Contracts.UseCases.Cart.AddToCart;
using Locamart.Nava.Application.Contracts.UseCases.Cart.GetUserCarts;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Locamart.Nava.Adapter.Http.Cart;


[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[Route("api/cart")]
public class CartController(IMediator mediator, ICurrentUser currentUser) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddToCart(AddToCartRequestModel request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddToCartCommand>();

        command.UserId = currentUser.UserId.Value;

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserCarts(CancellationToken cancellationToken)
    {
        var query = new GetUserCartsQuery()
        {
            UserId = currentUser.UserId
        };

        var result = await mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}

