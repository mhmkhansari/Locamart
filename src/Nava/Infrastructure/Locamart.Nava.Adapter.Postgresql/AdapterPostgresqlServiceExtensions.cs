using Locamart.Dina.Abstracts;
using Locamart.Nava.Adapter.Postgresql.QueryServices;
using Locamart.Nava.Adapter.Postgresql.Repositories;
using Locamart.Nava.Adapter.Postgresql.Seeders;
using Locamart.Nava.Adapter.Postgresql.Stores;
using Locamart.Nava.Application.Contracts.Services;
using Locamart.Nava.Domain.Entities.Cart.Abstracts;
using Locamart.Nava.Domain.Entities.Comment.Abstracts;
using Locamart.Nava.Domain.Entities.Inventory.Abstracts;
using Locamart.Nava.Domain.Entities.Product.Abstracts;
using Locamart.Nava.Domain.Entities.ProductCategory.Abstracts;
using Locamart.Nava.Domain.Entities.Store.Abstracts;
using Locamart.Nava.Domain.Entities.StoreCategory.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Locamart.Nava.Adapter.Postgresql;

public static class AdapterPostgresqlServiceExtensions
{
    public static IServiceCollection AddPostgresqleServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<LocamartNavaDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Postgres");
            options.UseNpgsql(connectionString);
        });

        services.AddDbContext<LocamartNavaQueryDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Postgres");
            options.UseNpgsql(connectionString);
        });

        services.AddDbContext<LocamartIdentityDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Postgres");
            options.UseNpgsql(connectionString);
            options.UseOpenIddict();
        });

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IStoreRepository, StoreRepository>();

        services.AddScoped<IStoreCategoryRepository, StoreCategoryRepository>();

        services.AddScoped<ICommentRepository, CommentRepository>();

        services.AddScoped<IInventoryRepository, InventoryRepository>();

        services.AddScoped<ICartRepository, CartRepository>();

        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

        services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();

        services.AddScoped<ICommentQueryService, CommentQueryService>();

        services.AddScoped<IStoreCategoryQueryService, StoreCategoryQueryService>();

        services.AddHostedService<ClientSeeder>();

        services.AddScoped<IUserStore, UserStore>();

        return services;
    }
}
