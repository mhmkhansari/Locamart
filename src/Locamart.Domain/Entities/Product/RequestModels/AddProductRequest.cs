using Locamart.Shared.ValueObjects;

namespace Locamart.Domain.Entities.Product.RequestModels;

public class AddProductRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public List<Image> Images { get; set; } = [];
}
