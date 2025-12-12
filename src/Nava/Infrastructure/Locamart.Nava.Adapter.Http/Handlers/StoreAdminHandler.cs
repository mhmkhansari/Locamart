using Locamart.Nava.Adapter.Http.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Locamart.Nava.Adapter.Http.Handlers;

public class StoreAdminHandler : AuthorizationHandler<StoreAdminRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StoreAdminHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        StoreAdminRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            return Task.CompletedTask;

        var routeStoreId = httpContext.GetRouteData()
            .Values["storeId"]?.ToString();

        if (routeStoreId == null)
            return Task.CompletedTask;

        var userStoreIds = context.User.Claims
            .Where(c => c.Type == "store-admin")
            .Select(c => c.Value);

        if (userStoreIds.Contains(routeStoreId))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
