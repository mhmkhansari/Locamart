using Microsoft.AspNetCore.Authorization;

namespace Locamart.Nava.Adapter.Http.Attributes;

public class StoreAdminAttribute : AuthorizeAttribute
{
    public StoreAdminAttribute() : base("StoreAdminPolicy") { }
}
