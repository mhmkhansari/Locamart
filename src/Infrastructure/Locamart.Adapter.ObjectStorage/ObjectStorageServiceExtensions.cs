using Locamart.Application.Contracts.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace Locamart.Adapter.ObjectStorage;
public static class ObjectStorageServiceExtensions
{
    public static IServiceCollection AddObjectStorageServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(sp =>
        {

            var endpoint = configuration["MinioConfig:Endpoint"];
            var accessKey = configuration["MinioConfig:Username"];
            var secretKey = configuration["MinioConfig:Password"];

            return new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .Build();
        });

        services.AddScoped<IImageStorageService, MinioImageStorageService>();
        return services;
    }
}
