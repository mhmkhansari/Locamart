using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Application.Contracts.Services;
using Locamart.Nava.Application.Contracts.UseCases.Cart.AddToCart;
using Locamart.Nava.Domain.Entities.Cart;
using Locamart.Nava.Domain.Entities.Cart.Abstracts;
using Locamart.Nava.Domain.Entities.Inventory.Abstracts;
using Locamart.Nava.Domain.Entities.Inventory.ValueObjects;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;

namespace Locamart.Nava.Application.UseCases.Cart.AddToCart;

public class AddToCartCommandHandler(IInventoryQueryService inventoryQueryService,
    ICartRepository cartRepository,
    IInventoryRepository inventoryRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddToCartCommand, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var userId = UserId.Create(request.UserId);

        if (userId.IsFailure)
            return userId.Error;

        var inventoryId = InventoryId.Create(request.InventoryId);

        if (inventoryId.IsFailure)
            return inventoryId.Error;

        var inventory = await inventoryRepository.GetById(inventoryId.Value, cancellationToken);

        if (inventory is null)
            return Error.Create("invalid_inventory_id", "Invalid inventory id");

        if (request.Quantity > inventory.Atp)
            return Error.Create("requested_quantity_exceeds_atp", "Requested quantity exceeds atp");

        var rawStoreId = await inventoryQueryService.GetStoreByInventoryId(request.InventoryId, cancellationToken);

        if (rawStoreId is null || rawStoreId == Guid.Empty)
            return Error.Create("invalid_store_id", "Invalid store id!");

        var storeId = StoreId.Create(rawStoreId.Value);

        if (storeId.IsFailure)
            return storeId.Error;

        var cartExists = await cartRepository.GetByStoreId(storeId.Value, cancellationToken);


        if (cartExists is null)
        {
            var newCart = CartEntity.Create(userId.Value, storeId.Value);

            if (newCart.IsFailure)
                return newCart.Error;

            newCart.Value.AddItem(inventoryId.Value, request.Quantity);

            await cartRepository.AddAsync(newCart.Value, cancellationToken);

        }
        else
        {
            cartExists.AddItem(inventoryId.Value, request.Quantity);

            cartRepository.Update(cartExists);
        }

        await unitOfWork.CommitAsync(cancellationToken);

        return UnitResult.Success<Error>();

    }
}

