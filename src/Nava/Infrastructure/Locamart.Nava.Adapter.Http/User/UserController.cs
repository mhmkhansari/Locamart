using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Locamart.Dina.Abstracts;
using Locamart.Nava.Adapter.Http.User.RequestModels;
using Locamart.Nava.Application.Contracts.UseCases.User.AddUserAddress;
using Locamart.Nava.Application.Contracts.UseCases.User.GetUserAddresses;
using Locamart.Nava.Application.Contracts.UseCases.User.Login;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Locamart.Nava.Adapter.Http.User;

[ApiController]
[Route("api/users")]
public class UserController(IMediator mediator, IHttpClientFactory httpClientFactory, ICurrentUser currentUser) : ControllerBase
{

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Register(LoginRequestModel request, CancellationToken cancellationToken)
    {

        var command = new LoginCommand()
        {
            MobileNumber = request.MobileNumber
        };

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost]
    [Route("verify")]
    public async Task<IActionResult> VerifyOtp(VerifyOtpRequestModel request,
        CancellationToken cancellationToken)
    {

        var httpClient = httpClientFactory.CreateClient();
        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "otp" },
            { "username", request.MobileNumber },
            { "client_id", "web-client" },
            { "client_secret", "901564A5-E7FE-42CB-B10D-61EF6A8F3654" },
            { "otp_code", request.OtpCode },
            { "challenge_id", request.ChallengeId },
            { "scope", "api offline_access" }
        };

        var content = new FormUrlEncodedContent(parameters);
        var response = await httpClient.PostAsync("https://localhost:7046/connect/token", content, cancellationToken);

        var tokenResponseJson = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
            return BadRequest(tokenResponseJson);

        return Ok(tokenResponseJson);
    }

    [HttpPost]
    [Route("address")]
    public async Task<IActionResult> AddUserAddress(AddUserAddressRequestModel request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddUserAddressCommand>();

        command.UserId = currentUser.UserId;

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpGet]
    [Route("address")]
    public async Task<IActionResult> GetUserAddresses(CancellationToken cancellationToken)
    {
        var query = new GetUserAddressesQuery()
        {
            UserId = currentUser.UserId
        };

        var result = await mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

}
