using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Locamart.Application.Contracts.Services;
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
                    .DefaultIndex(configuration["ElasticsearchSettings:DefaultIndex"]!)
                    .Authentication(new BasicAuthentication(configuration["ElasticsearchSettings:Username"]!, configuration["ElasticsearchSettings:Password"]!));

                return new ElasticsearchClient(settings);
            });

            services.AddScoped<ISearchService, ElasticsearchClientService>();

            return services;
        }
    }



}

