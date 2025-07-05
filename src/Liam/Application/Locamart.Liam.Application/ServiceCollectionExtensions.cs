using Locamart.Liam.Application.Contracts.Services;
using Locamart.Liam.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Locamart.Liam.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLiamApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILiamIdentityService, LiamIdentityService>();

        return services;
    }
}
