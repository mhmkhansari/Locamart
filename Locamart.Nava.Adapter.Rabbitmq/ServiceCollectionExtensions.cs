using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Locamart.Nava.Adapter.Rabbitmq;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitmqServices(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq://localhost", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });
        });

        return service;
    }
}

