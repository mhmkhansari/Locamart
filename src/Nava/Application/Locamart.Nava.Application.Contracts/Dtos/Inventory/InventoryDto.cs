namespace Locamart.Nava.Application.Contracts.Dtos.Inventory;

public class InventoryDto
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public Guid StoreId { get; init; }
    public int AvailableQuantity { get; init; }
    public int ReservedQuantity { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; } = default!;
}

