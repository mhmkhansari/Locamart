using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Nava.Application.Contracts.Dtos.User;

namespace Locamart.Nava.Application.Contracts.Services;

public interface IUserStore
{
    Task<Result<UserDto?, Error>> GetUserById(Guid id);
    Task<Result<UserDto?, Error>> GetUserByPhoneNumber(string phoneNumber);
    Task<UnitResult<Error>> AddClaimAsync(Guid userId, string claimName, string claimValue);
}
