using Locamart.Liam.Adapter.Postgresql;
using Locamart.Liam.Application.Contracts.UseCases.RegisterUser;
using Locamart.Liam.Application.Contracts.UseCases.VerifyOtp;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Locamart.Liam.Adapter.Http.User;

[ApiController]
[Route("api/users")]
public class UserController(IMediator mediator, IHttpClientFactory httpClientFactory) : ControllerBase
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
    public async Task<IActionResult> VerifyOtp(VerifyOtpRequestModel request,
        CancellationToken cancellationToken)
    {

        var httpClient = httpClientFactory.CreateClient();
        var parameters = new Dictionary<string, string>
            {
                { "grant_type", "otp" },
                { "username", request.UserId.ToString() },
                { "client_id", "web-client" },
                { "client_secret", "901564A5-E7FE-42CB-B10D-61EF6A8F3654" },
                {"otp_code", request.OtpCode},
                { "scope", "api" }
            };

        var content = new FormUrlEncodedContent(parameters);
        var response = await httpClient.PostAsync("http://localhost:5103/connect/token", content, cancellationToken);

        var tokenResponseJson = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
            return BadRequest(tokenResponseJson);

        return Ok(tokenResponseJson);
    }
}


