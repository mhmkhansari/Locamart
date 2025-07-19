using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Liam.Application.Contracts.Dtos.User;

namespace Locamart.Liam.Application.Contracts.Services;

public interface IUserStore
{
    Task<Result<UserDto?, Error>> GetUserById(Guid id);
    Task<Result<UserDto?, Error>> GetUserByPhoneNumber(string phoneNumber);
}

