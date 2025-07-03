using Microsoft.AspNetCore.Identity;

namespace Locamart.Liam;

public class LiamIdentityService(UserManager<IdentityUser> userManager)
{
    public Task CreateUser()
    {

    }
}

