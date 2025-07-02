using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Locamart.Adapter.Elasticsearch
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAdapterElasticsearchServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(sp =>
            {
                var settings = new ElasticsearchClientSettings(new Uri(configuration["ElasticsearchSettings:Url"]!))
                    .DefaultIndex(configuration["ElasticsearchSettings:DefaultIndex"]!);

                return new ElasticsearchClient(settings);
            });

            return services;
        }
    }



}

