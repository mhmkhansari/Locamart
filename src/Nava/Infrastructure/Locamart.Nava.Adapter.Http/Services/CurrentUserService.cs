using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.Services;
using Microsoft.AspNetCore.Http;
using OpenIddict.Abstractions;
using System.Security.Claims;

namespace Locamart.Nava.Adapter.Http.Services;

/*public class CurrentUserService : ICurrentUser
{
    public Guid UserId { get; }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User
                   ?? throw new UnauthorizedAccessException("No HTTP context available");

        if (!user.Identity?.IsAuthenticated ?? true)
            throw new UnauthorizedAccessException("User is not authenticated");

        var claim = user.FindFirst(OpenIddictConstants.Claims.Subject)?.Value
                    ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (claim == null || !Guid.TryParse(claim, out var id))
            throw new UnauthorizedAccessException("Invalid or missing user ID claim");

        UserId = id;
    }
}*/
