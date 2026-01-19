using Elastic.Clients.Elasticsearch;

namespace Locamart.Nava.Adapter.Elasticsearch.Models;

public class ProductModel
{
    public string ProductId { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<string> Images { get; set; }

    public List<InventoryModel> StoreInventory { get; set; } = new List<InventoryModel>();
}

public class InventoryModel
{
    public string StoreId { get; set; } = default!;
    public string StoreName { get; set; } = default!;
    public string? StoreIdentifier { get; set; }

    public double Price { get; set; }
    public int AvailableQuantity { get; set; }
    public int ReservedQuantity { get; set; }
    public int Atp { get; set; }

    public GeoLocation StoreLocation { get; set; } = default!;
}


