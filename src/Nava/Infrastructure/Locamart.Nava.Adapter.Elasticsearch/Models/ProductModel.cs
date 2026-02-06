using System.Text.Json.Serialization;

namespace Locamart.Nava.Adapter.Elasticsearch.Models;

public class ProductModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    [JsonPropertyName("images")]
    public List<ProductImageModel> Images { get; set; } = new();

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("createdBy")]
    public string CreatedBy { get; set; } = null!;

    /*    [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("updatedBy")]
        public string? UpdatedBy { get; set; }*/

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    [JsonPropertyName("storeInventory")]
    public List<InventoryModel> StoreInventory { get; set; } = new();
}

public class ProductImageModel
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = default!;

    /*    [JsonPropertyName("isPrimary")]
        public bool IsPrimary { get; set; }*/

    [JsonPropertyName("order")]
    public int Order { get; set; }
}


