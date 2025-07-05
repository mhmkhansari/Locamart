using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Locamart.Adapter.Elasticsearch;
using Locamart.Nava.Application.Contracts.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Locamart.Nava.Adapter.Elasticsearch
{
    public static class AdapterElasticServiceCollectionExtensions
    {
        public static IServiceCollection AddAdapterElasticsearchServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(sp =>
            {
                var settings = new ElasticsearchClientSettings(new Uri(configuration["ElasticsearchSettings:Url"]!))
                    .DefaultIndex(configuration["ElasticsearchSettings:DefaultIndex"]!)
                    .Authentication(new BasicAuthentication(configuration["ElasticsearchSettings:Username"]!, configuration["ElasticsearchSettings:Password"]!));

                return new ElasticsearchClient(settings);
            });

            services.AddScoped<ISearchService, ElasticsearchClientService>();

            return services;
        }

        public static async Task InitializeAdapterElasticsearchAsync(IServiceProvider serviceProvider)
        {
            var client = serviceProvider.GetRequiredService<ElasticsearchClient>();
            await IndexInitialization.EnsureProductIndexExistsAsync(client);
        }
    }



}

