using System.Text.Json.Serialization;

namespace Locamart.Nava.Adapter.Elasticsearch.Models;

public class InventoryModel
{
    [JsonPropertyName("storeId")]
    public string StoreId { get; set; } = null!;

    [JsonPropertyName("storeName")]
    public string StoreName { get; set; } = null!;

    [JsonPropertyName("storeIdentifier")]
    public string StoreIdentifier { get; set; } = null!;

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("availableQuantity")]
    public int AvailableQuantity { get; set; }

    [JsonPropertyName("reservedQuantity")]
    public int ReservedQuantity { get; set; }

    [JsonPropertyName("storeLocation")]
    public GeoLocationRest StoreLocation { get; set; } = null!;
}

public class GeoLocationRest(double lat, double lon)
{
    [JsonPropertyName("lat")]
    public double Lat { get; set; } = lat;

    [JsonPropertyName("lon")]
    public double Lon { get; set; } = lon;
}