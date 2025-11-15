using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Liam.Application.Contracts.Dtos.User;
using Locamart.Liam.Application.Contracts.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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

    public async Task<UnitResult<Error>> AddClaimAsync(Guid userId, string claimName, string claimValue)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            return Error.Create("user_not_found", "User not found!");

        var claim = new Claim(claimName, claimValue);

        var addClaimResult = await userManager.AddClaimAsync(user, claim);

        if (!addClaimResult.Succeeded)
            return Error.Create("failed_to_add_claim", "Failed to add claim!");

        return UnitResult.Success<Error>();

    }
}

