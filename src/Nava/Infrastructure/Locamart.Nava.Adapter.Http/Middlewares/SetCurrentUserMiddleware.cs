using Locamart.Dina;
using Locamart.Dina.ValueObjects;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Locamart.Nava.Adapter.Http.Middlewares;

public class SetCurrentUserMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext, CurrentUserContext context)
    {
        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is not null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            context.UserId = UserId.Create(userId).Value;
        }

        await next(httpContext);
    }
}
