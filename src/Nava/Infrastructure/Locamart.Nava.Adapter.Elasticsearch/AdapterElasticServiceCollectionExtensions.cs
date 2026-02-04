using Locamart.Nava.Application.Contracts.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Locamart.Nava.Adapter.Elasticsearch
{
    public static class AdapterElasticServiceCollectionExtensions
    {
        public static IServiceCollection AddAdapterElasticsearchServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<ElasticsearchHttpClient>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:9200");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddScoped<ISearchService, ElasticsearchClientService>();

            return services;
        }

        public static async Task InitializeAdapterElasticsearchAsync(
            IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var client = scope.ServiceProvider
                .GetRequiredService<ElasticsearchHttpClient>();

            await ElasticsearchIndexBootstrapper
                .EnsureProductIndexExistsAsync(client);
        }
    }



}

