namespace Locamart.Nava.Adapter.Http.Store.RequestModels;

public record CreateStoreRequest
{
    public string Name { get; set; }
    public Guid CategoryId { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

}

