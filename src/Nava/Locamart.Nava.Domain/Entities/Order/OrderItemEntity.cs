using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Domain.Entities.Inventory;
using Locamart.Nava.Domain.Entities.Inventory.ValueObjects;
using Locamart.Nava.Domain.Entities.Order.ValueObjects;
using Locamart.Nava.Domain.Entities.Product.ValueObjects;

namespace Locamart.Nava.Domain.Entities.Order;

public sealed class OrderItemEntity : AuditableEntity<OrderItemId>
{
    public OrderId OrderId { get; private set; }

    public ProductId ProductId { get; private set; }
    public InventoryId InventoryId { get; private set; }

    public int Quantity { get; private set; }

    // Snapshot values
    public Price UnitPrice { get; private set; }
    public Money TotalPrice => UnitPrice.ToMoney(Quantity);

    private OrderItemEntity(OrderItemId id) : base(id) { }

    private OrderItemEntity(
        OrderItemId id,
        OrderId orderId,
        InventoryId inventoryId,
        int quantity,
        Price unitPrice
    ) : base(id)
    {
        OrderId = orderId;
        InventoryId = inventoryId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public static Result<OrderItemEntity, Error> Create(
        OrderId orderId,
        InventoryEntity inventory,
        int quantity
    )
    {
        if (quantity <= 0)
            return Error.Create("invalid_quantity", "Quantity must be greater than zero");

        var reserveResult = inventory.Reserve(quantity);
        if (reserveResult.IsFailure)
            return reserveResult.Error;

        var orderItemIdResult = OrderItemId.Create(Guid.NewGuid());
        if (orderItemIdResult.IsFailure)
            return orderItemIdResult.Error;

        return new OrderItemEntity(
            orderItemIdResult.Value,
            orderId,
            inventory.Id,
            quantity,
            inventory.Price
        );
    }

    public UnitResult<Error> CommitInventory(InventoryEntity inventory)
    {
        if (inventory.Id != InventoryId)
            return Error.Create("inventory_mismatch", "Inventory does not match order item");

        return inventory.CommitReservation(Quantity);
    }

    public UnitResult<Error> ReleaseInventory(InventoryEntity inventory)
    {
        if (inventory.Id != InventoryId)
            return Error.Create("inventory_mismatch", "Inventory does not match order item");

        return inventory.ReleaseReservation(Quantity);
    }
}
