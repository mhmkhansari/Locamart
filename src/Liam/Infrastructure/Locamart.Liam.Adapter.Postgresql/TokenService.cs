/*using Locamart.Liam.Domain;
using Locamart.Liam.Domain.Abstractions;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace Locamart.Liam.Adapter.Postgresql;

public class TokenService : ITokenService
{
    public Task<object> IssueTokensAsync(User user)
    {
        var claims = new[]
        {
            new Claim(OpenIddictConstants.Claims.Subject, user.Id.ToString()),
            new Claim(OpenIddictConstants.Claims.PhoneNumber, user.MobileNumber.ToString())
        };

        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme));
        var rs = new Signin
        return Task.FromResult<object>(new SignInResult(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme));
    }

    public class SignInResult(ClaimsPrincipal principal, string authenticationScheme)
    {
        public ClaimsPrincipal Principal { get; } = principal;
        public string AuthenticationScheme { get; } = authenticationScheme;
    }
}
*/
