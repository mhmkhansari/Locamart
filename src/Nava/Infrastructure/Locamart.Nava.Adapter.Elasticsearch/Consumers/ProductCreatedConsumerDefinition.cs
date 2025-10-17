using MassTransit;

namespace Locamart.Nava.Adapter.Elasticsearch.Consumers;

public sealed class ProductCreatedConsumerDefinition
    : ConsumerDefinition<ProductCreatedConsumer>
{
    public ProductCreatedConsumerDefinition()
    {
        EndpointName = "products-indexer";
        ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpoint,
        IConsumerConfigurator<ProductCreatedConsumer> consumer,
        IRegistrationContext context)
    {
        endpoint.UseInMemoryOutbox(context);
    }
}

public static class ElasticsearchMessagingModule
{
    public static void AddElasticsearchMessaging(this IBusRegistrationConfigurator cfg)
    {
        cfg.AddConsumer<ProductCreatedConsumer, ProductCreatedConsumerDefinition>();
    }
}
