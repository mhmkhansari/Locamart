using Elastic.Clients.Elasticsearch;

namespace Locamart.Adapter.Elasticsearch.Models;

public class ProductSearch
{
    public string ProductId { get; set; }
    public string StoreId { get; set; }
    public string StoreName { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public GeoLocation StoreLocation { get; set; }
}

