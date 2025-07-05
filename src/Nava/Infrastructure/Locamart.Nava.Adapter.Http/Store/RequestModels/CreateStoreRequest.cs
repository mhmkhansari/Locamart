namespace Locamart.Adapter.Http.Store.RequestModels;

public record CreateStoreRequest
{
    public string Name { get; init; }
    public Guid CategoryId { get; init; }

}

