using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;

namespace Locamart.Adapter.Elasticsearch;

public class IndexInitialization
{
    public static async Task EnsureProductIndexExistsAsync(ElasticsearchClient client)
    {
        var exists = await client.Indices.ExistsAsync("products");
        if (exists.Exists)
            return;

        var createResponse = await client.Indices.CreateAsync("products", c => new CreateIndexRequest("products")
        {
            Mappings = new TypeMapping
            {
                Properties = new Properties
                {
                    ["product_id"] = new KeywordProperty(),
                    ["store_id"] = new KeywordProperty(),
                    ["product_name"] = new TextProperty(),
                    ["description"] = new TextProperty(),
                    ["price"] = new DoubleNumberProperty(),
                    ["store_location"] = new GeoPointProperty()
                }
            }
        });

        if (!createResponse.IsValidResponse)
            throw new Exception("Index creation failed: " + createResponse.DebugInformation);
    }
}

