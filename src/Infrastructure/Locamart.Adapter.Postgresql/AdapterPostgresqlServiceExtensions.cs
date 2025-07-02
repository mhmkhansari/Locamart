using Locamart.Adapter.Postgresql.Repositories;
using Locamart.Domain.Entities.Product.Abstracts;
using Locamart.Domain.Entities.Store.Abstracts;
using Locamart.Domain.Entities.StoreCategory.Abstracts;
using Locamart.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Locamart.Adapter.Postgresql;

public static class AdapterPostgresqlServiceExtensions
{
    public static IServiceCollection AddPostgresqleServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LocamartDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Postgres");
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IStoreRepository, StoreRepository>();

        services.AddScoped<IStoreCategoryRepository, StoreCategoryRepository>();

        services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();

        return services;
    }

}
