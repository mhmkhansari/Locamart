using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Locamart.Liam.Adapter.Postgresql;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLiamPostgresServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LiamDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Liam");
            options.UseNpgsql(connectionString);
            options.UseOpenIddict();
        });

        services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 0;
                options.Password.RequiredUniqueChars = 0;
            })
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

