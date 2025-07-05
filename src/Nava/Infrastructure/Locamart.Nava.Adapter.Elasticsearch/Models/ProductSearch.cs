using Elastic.Clients.Elasticsearch;

namespace Locamart.Adapter.Elasticsearch.Models;

public class ProductSearch
{
    public string productId { get; set; }
    public string storeId { get; set; }
    public string storeName { get; set; }
    public string productName { get; set; }
    public string description { get; set; }
    //public float Price { get; set; }
    public GeoLocation storeLocation { get; set; }
    //public
}

