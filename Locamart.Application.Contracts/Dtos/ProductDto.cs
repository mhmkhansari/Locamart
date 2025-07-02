namespace Locamart.Application.Contracts.Dtos;

public class ProductDto
{
    public Guid ProductId { get; set; }
    public Guid StoreId { get; set; }
    public string StoreName { get; set; }
    public string ProductName { get; set; }
    public double DistanceInKilometers { get; set; }
}

