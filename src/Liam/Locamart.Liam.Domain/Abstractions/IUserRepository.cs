using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Liam.Domain.ValueObjects;

namespace Locamart.Liam.Domain.Abstractions;

public interface IUserRepository
{
    Task<bool> ExistsAsync(MobileNumber mobileNumber);
    Task<UnitResult<Error>> CreateAsync(MobileNumber mobileNumber);
}
