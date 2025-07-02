namespace Locamart.Domain.Dtos;

public record GetProductWithinDistanceDto
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string StoreName { get; set; }
    public Guid StoreId { get; set; }
    public long Distance { get; set; }
}

