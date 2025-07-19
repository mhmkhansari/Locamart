using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Liam.Application.Contracts.Dtos.User;
using Locamart.Liam.Application.Contracts.Services;
using Microsoft.AspNetCore.Identity;

namespace Locamart.Liam.Adapter.Postgresql.Stores;

public class UserStore(UserManager<IdentityUser> userManager) : IUserStore
{
    public async Task<Result<UserDto?, Error>> GetUserById(Guid id)
    {
        var result = await userManager.FindByIdAsync(id.ToString());

        if (result is null)
            return Result.Success<UserDto?, Error>(null);

        return new UserDto()
        {
            Id = result.Id,
            Email = result.Email,
            PhoneNumber = result.PhoneNumber,
            UserName = result.UserName
        };
    }

    public async Task<Result<UserDto?, Error>> GetUserByPhoneNumber(string phoneNumber)
    {
        var result = await userManager.FindByNameAsync(phoneNumber);

        if (result is null)
            return Result.Success<UserDto?, Error>(null);

        return new UserDto()
        {
            Id = result.Id,
            Email = result.Email,
            PhoneNumber = result.PhoneNumber,
            UserName = result.UserName
        };
    }
}

