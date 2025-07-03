using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Locamart.Liam;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLiamServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LiamDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Liam");
            options.UseNpgsql(connectionString);
        });

        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<LiamDbContext>()
            .AddDefaultTokenProviders();

        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<LiamDbContext>();
            })
            .AddServer(options =>
            {
                options.SetTokenEndpointUris("/connect/token")
                    .SetAuthorizationEndpointUris("/connect/authorize")
                    .AllowPasswordFlow()
                    .AllowRefreshTokenFlow();

                options.RegisterScopes("api");

                options.AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();

                options.UseAspNetCore()
                    .EnableTokenEndpointPassthrough()
                    .EnableAuthorizationEndpointPassthrough();
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });

        services.AddHostedService<ClientSeeder>();

        return services;
    }
}

