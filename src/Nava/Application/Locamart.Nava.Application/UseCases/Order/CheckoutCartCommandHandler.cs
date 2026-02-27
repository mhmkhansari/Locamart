using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Application.Contracts.Services;
using Locamart.Nava.Application.Contracts.UseCases.Order;
using Locamart.Nava.Domain.Entities.Order.Abstracts;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;

namespace Locamart.Nava.Application.UseCases.Order;

public class CheckoutCartCommandHandler(ICartQueryService cartQueryService,
                                        IOrderRepository orderRepository,
                                        IUnitOfWork unitOfWork)
    : ICommandHandler<CheckoutCartCommand, Result<CheckoutCartCommandResponse, Error>>
{
    public async Task<Result<CheckoutCartCommandResponse, Error>> Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
    {
        await using var tx = await unitOfWork.BeginTransactionAsync(cancellationToken);

        var cart = await cartQueryService.GetActiveForUserAndStore(request.UserId, request.StoreId, lockForUpdate: true, cancellationToken);

        if (cart is null)
            return Error.Create("cart_not_found", "No active cart found for this store");

        if (!cart.Items.Any())
            return Error.Create("empty_cart", "Cannot checkout an empty cart");

        var userId = UserId.Create(request.UserId);

        if (userId.IsFailure)
            return userId.Error;

        var storeId = StoreId.Create(request.StoreId);

        if (storeId.IsFailure)
            return storeId.Error;

        var hasActiveOrder = await orderRepository.ExistsActiveForUserAndStore(userId.Value, storeId.Value, cancellationToken);

        if (hasActiveOrder)
            return Error.Create("active_order_exists", "You already have an active order for this store");

        return new Result<CheckoutCartCommandResponse, Error>();
        // 3️⃣ Create Order (snapshot) var orderResult = OrderEntity.Create( userId: command.UserId, storeId: command.StoreId, createdAt: _clock.UtcNow ); if (orderResult.IsFailure) return orderResult.Error; var order = orderResult.Value; // 4️⃣ Convert cart items → order items (NO reservation yet) foreach (var cartItem in cart.Items) { var addItemResult = order.AddItem( productId: cartItem.ProductId, inventoryId: cartItem.InventoryId, quantity: cartItem.Quantity, unitPrice: cartItem.UnitPriceSnapshot ); if (addItemResult.IsFailure) return addItemResult.Error; } // 5️⃣ Persist order await _orderRepository.Add(order, ct); // 6️⃣ Optional: mark cart as checked out (or keep as read-only snapshot) cart.MarkCheckedOut(); await _cartRepository.Update(cart, ct); await tx.CommitAsync(ct); return order.Id; }
    }
}

