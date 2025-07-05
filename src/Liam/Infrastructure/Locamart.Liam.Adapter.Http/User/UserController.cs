using Locamart.Liam.Application.Contracts.UseCases.RegisterUser;
using Locamart.Liam.Application.Contracts.UseCases.VerifyOtp;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Locamart.Liam.Adapter.Http.User;

[ApiController]
[Route("api/users")]
public class UserController(IMediator mediator) : ControllerBase
{

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(string mobileNumber, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand()
        {
            MobileNumber = mobileNumber
        };

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost]
    [Route("verify-otp")]
    public async Task<IActionResult> VerifyOtp(Guid tempUserId, string otpCode,
        CancellationToken cancellationToken)
    {
        var command = new VerifyOtpCommand()
        {
            UserId = tempUserId,
            OtpCode = otpCode
        };

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}

