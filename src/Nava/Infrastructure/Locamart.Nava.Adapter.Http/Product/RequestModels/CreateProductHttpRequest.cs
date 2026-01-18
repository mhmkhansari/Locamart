namespace Locamart.Adapter.Http.Product.RequestModels;

public record CreateProductHttpRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
}
