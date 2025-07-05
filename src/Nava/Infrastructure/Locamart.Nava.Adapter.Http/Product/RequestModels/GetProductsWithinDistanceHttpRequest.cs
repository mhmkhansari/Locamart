namespace Locamart.Adapter.Http.Product.RequestModels;

public class GetProductsWithinDistanceHttpRequest
{
    public long Distance { get; set; }
    public string Product { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

