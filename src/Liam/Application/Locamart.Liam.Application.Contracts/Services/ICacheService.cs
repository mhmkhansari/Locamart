using CSharpFunctionalExtensions;
using Locamart.Dina;

namespace Locamart.Liam.Application.Contracts.Services;

public interface ICacheService
{
    Task<UnitResult<Error>> SetAsync(string key, string value, TimeSpan? expiry = null);
    Task<Result<string?, Error>> GetAsync(string key);
    Task<UnitResult<Error>> RemoveAsync(string key);
    Task<Result<bool, Error>> ExistsAsync(string key);
    Task<UnitResult<Error>> PushToListAsync(string queueName, string key);
}

