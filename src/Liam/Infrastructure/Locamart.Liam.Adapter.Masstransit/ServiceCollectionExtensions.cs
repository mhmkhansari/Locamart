using Locamart.Liam.Adapter.Masstransit.Consumers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Locamart.Liam.Adapter.Masstransit;

public static class ServiceCollectionExtensions
{
    public static void AddLiamMasstransit(this IBusRegistrationConfigurator cfg)
    {

        cfg.AddConsumer<StoreCreatedConsumer>();

        cfg.AddEntityFrameworkOutbox<LocamartLiamAdapterMasstransitDbContext>(o =>
        {
            o.UsePostgres();
            o.UseBusOutbox();
        });

    }
}

