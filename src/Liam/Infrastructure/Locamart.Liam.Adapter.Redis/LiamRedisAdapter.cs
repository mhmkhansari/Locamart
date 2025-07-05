using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Liam.Application.Contracts.Services;
using StackExchange.Redis;

namespace Locamart.Liam.Adapter.Redis;

public class LiamRedisAdapter(IConnectionMultiplexer connection) : ICacheService
{
    private readonly IDatabase _db = connection.GetDatabase();

    public async Task<UnitResult<Error>> SetAsync(string key, string value, TimeSpan? expiry = null)
    {
        if (string.IsNullOrEmpty(key))
            return Error.Create("redis_key_not_provided", "Redis key not provided");

        await _db.StringSetAsync(key, value, expiry);

        return UnitResult.Success<Error>();
    }

    public async Task<Result<string?, Error>> GetAsync(string key)
    {
        if (string.IsNullOrEmpty(key))
            return Error.Create("redis_key_not_provided", "Redis key not provided");

        var value = await _db.StringGetAsync(key);
        return value.HasValue ? value.ToString() : null;
    }

    public async Task<UnitResult<Error>> RemoveAsync(string key)
    {
        if (string.IsNullOrEmpty(key))
            return Error.Create("redis_key_not_provided", "Redis key not provided");

        await _db.KeyDeleteAsync(key);

        return UnitResult.Success<Error>();
    }

    public async Task<Result<bool, Error>> ExistsAsync(string key)
    {
        if (string.IsNullOrEmpty(key))
            return Error.Create("redis_key_not_provided", "Redis key not provided");

        return await _db.KeyExistsAsync(key);
    }

    public async Task<UnitResult<Error>> PushToListAsync(string queueName, string key)
    {
        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(queueName))
            return Error.Create("redis_key_or_queue_not_provided", "Redis key or queue not provided");

        await _db.ListRightPushAsync(queueName, key);

        return UnitResult.Success<Error>();
    }
}

