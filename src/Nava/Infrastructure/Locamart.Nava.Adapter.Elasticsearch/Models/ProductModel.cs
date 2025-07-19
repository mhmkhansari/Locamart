using Elastic.Clients.Elasticsearch;

namespace Locamart.Nava.Adapter.Elasticsearch.Models;

public class ProductModel
{
    public string productId { get; set; }
    public string storeId { get; set; }
    public string storeName { get; set; }
    public string productName { get; set; }
    public decimal price { get; set; }
    public GeoLocation storeLocation { get; set; }
}

