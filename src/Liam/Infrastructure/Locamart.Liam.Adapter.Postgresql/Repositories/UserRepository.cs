using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Liam.Domain.Abstractions;
using Locamart.Liam.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Locamart.Liam.Adapter.Postgresql.Repositories;

public class UserRepository(UserManager<IdentityUser> userManager) : IUserRepository
{
    public Task<bool> ExistsAsync(MobileNumber mobileNumber)
    {
        throw new NotImplementedException();
    }

    public async Task<UnitResult<Error>> CreateAsync(MobileNumber mobileNumber)
    {
        /* var user = new IdentityUser { UserName = mobileNumber.Value, PhoneNumber = mobileNumber.Value };
         var result = await userManager.CreateAsync(user, null);

         if (!result.Succeeded)
             return Error.Create("error_create_new_user", result.Errors.ToString());*/
        throw new NotImplementedException();

    }
}

