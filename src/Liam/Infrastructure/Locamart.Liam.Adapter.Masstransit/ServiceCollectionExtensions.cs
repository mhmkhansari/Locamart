using Locamart.Liam.Adapter.Masstransit.Consumers;
using MassTransit;
namespace Locamart.Liam.Adapter.Masstransit;

public static class ServiceCollectionExtensions
{
    public static void AddLiamMasstransit(this IBusRegistrationConfigurator cfg)
    {
        cfg.AddConsumer<StoreCreatedConsumer>();
    }
}

