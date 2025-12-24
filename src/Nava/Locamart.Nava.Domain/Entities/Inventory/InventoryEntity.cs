using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Nava.Domain.Entities.Inventory.ValueObjects;
using Locamart.Nava.Domain.Entities.Product.ValueObjects;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Inventory;

public sealed class InventoryEntity : AuditableEntity<InventoryId>
{
    public ProductId ProductId { get; private set; }
    public StoreId StoreId { get; private set; }
    public int AvailableQuantity { get; private set; }
    public int ReservedQuantity { get; private set; }
    public Price Price { get; private set; }
    public int Atp => AvailableQuantity - ReservedQuantity;

    private InventoryEntity(InventoryId id) : base(id) { }
    private InventoryEntity(
        InventoryId id,
        ProductId productId,
        StoreId storeId,
        int availableQuantity,
        Price price
    ) : base(id)
    {
        ProductId = productId;
        StoreId = storeId;
        AvailableQuantity = availableQuantity;
        ReservedQuantity = 0;
        Price = price;
    }

    public static Result<InventoryEntity, Error> Create(
        ProductId productId,
        StoreId storeId,
        int initialQuantity,
        Price price
    )
    {
        if (initialQuantity < 0)
            return Error.Create("invalid_quantity", "Initial quantity cannot be negative");

        var inventoryIdResult = InventoryId.Create(Guid.NewGuid());
        if (inventoryIdResult.IsFailure)
            return inventoryIdResult.Error;

        return new InventoryEntity(
            inventoryIdResult.Value,
            productId,
            storeId,
            initialQuantity,
            price
        );
    }

    public UnitResult<Error> Reserve(int quantity)
    {
        if (quantity <= 0)
            return Error.Create("invalid_quantity", "Reservation quantity must be greater than zero");

        if (quantity > Atp)
            return Error.Create("insufficient_stock", "Not enough available stock");

        ReservedQuantity += quantity;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> ReleaseReservation(int quantity)
    {
        if (quantity <= 0)
            return Error.Create("invalid_quantity", "Release quantity must be greater than zero");

        if (quantity > ReservedQuantity)
            return Error.Create("invalid_release", "Cannot release more than reserved");

        ReservedQuantity -= quantity;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> CommitReservation(int quantity)
    {
        if (quantity <= 0)
            return Error.Create("invalid_quantity", "Commit quantity must be greater than zero");

        if (quantity > ReservedQuantity)
            return Error.Create("invalid_commit", "Not enough reserved stock to commit");

        ReservedQuantity -= quantity;
        AvailableQuantity -= quantity;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            return Error.Create("invalid_quantity", "Increase quantity must be greater than zero");

        AvailableQuantity += quantity;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            return Error.Create("invalid_quantity", "Decrease quantity must be greater than zero");

        if (quantity > Atp)
            return Error.Create("insufficient_stock", "Cannot reduce below reserved stock");

        AvailableQuantity -= quantity;

        return UnitResult.Success<Error>();
    }
    public void SetPrice(Price price)
    {
        Price = price;
    }

}
