using Locamart.Nava.Adapter.Elasticsearch.Definitions;

namespace Locamart.Nava.Adapter.Elasticsearch;

public static class ElasticsearchIndexBootstrapper
{
    public static async Task EnsureProductIndexExistsAsync(ElasticsearchHttpClient esClient)
    {
        const string indexName = "products";

        if (await esClient.IndexExistsAsync(indexName))
            return;

        await esClient.CreateIndexAsync(indexName, ProductIndexDefinition.Build());
    }
}
