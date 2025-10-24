using Locamart.Dina.Abstracts;
using Locamart.Dina.Infrastructure;
using Locamart.Nava.Adapter.Postgresql.QueryServices;
using Locamart.Nava.Adapter.Postgresql.Repositories;
using Locamart.Nava.Application.Contracts.Services;
using Locamart.Nava.Domain.Entities.Comment.Abstracts;
using Locamart.Nava.Domain.Entities.Product.Abstracts;
using Locamart.Nava.Domain.Entities.Store.Abstracts;
using Locamart.Nava.Domain.Entities.StoreCategory.Abstracts;
using MassTransit;
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

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IStoreRepository, StoreRepository>();

        services.AddScoped<IStoreCategoryRepository, StoreCategoryRepository>();

        services.AddScoped<ICommentRepository, CommentRepository>();


        services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();

        services.AddScoped<ICommentQueryService, CommentQueryService>();

        services.AddScoped<IIntegrationEventPublisher, MassTransitIntegrationEventPublisher>();

        return services;
    }

    public sealed class MassTransitIntegrationEventPublisher(IPublishEndpoint bus)
        : IIntegrationEventPublisher
    {
        public Task PublishAsync<T>(T evt, CancellationToken ct = default)
            where T : class, IIntegrationEvent
            => bus.Publish(evt, ct);
    }

}
