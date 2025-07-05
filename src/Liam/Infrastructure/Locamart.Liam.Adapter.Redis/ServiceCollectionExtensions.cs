using Locamart.Liam.Application.Contracts.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Locamart.Liam.Adapter.Redis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLiamRedisServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration["RedisSettings:Url"]!));

        services.AddScoped<ICacheService, LiamRedisAdapter>();

        return services;
    }
}


