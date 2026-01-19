namespace Locamart.Nava.Application.Contracts.Dtos.Search;

public class ProductSearchDto
{
    public Guid ProductId { get; set; }
    public string Description { get; set; }
    public string ProductName { get; set; }
    public IEnumerable<InventorySearchDto> StoreInventories { get; set; }

}

public class InventorySearchDto
{
    public string StoreId { get; set; } = default!;
    public string StoreName { get; set; } = default!;
    public string? StoreIdentifier { get; set; }
    public double Price { get; set; }
    public int Atp { get; set; }
    public double DistanceInKilometers { get; set; }
}

