using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.Services;
using Locamart.Nava.Application.Contracts.UseCases.Inventory;
using Locamart.Nava.Domain.Entities.Inventory;
using Locamart.Nava.Domain.Entities.Inventory.Abstracts;
using Locamart.Nava.Domain.Entities.Product.Abstracts;
using Locamart.Nava.Domain.Entities.Product.Enums;
using Locamart.Nava.Domain.Entities.Product.ValueObjects;
using Locamart.Nava.Domain.Entities.Store.Abstracts;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;

namespace Locamart.Nava.Application.UseCases.Inventory.AddInventory;

public sealed class AddInventoryCommandHandler(
    IInventoryRepository inventoryRepository,
    IProductRepository productRepository,
    IStoreRepository storeRepository,
    IUnitOfWork unitOfWork,
    IIntegrationEventPublisher eventPublisher
) : ICommandHandler<AddInventoryCommand, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(
        AddInventoryCommand request,
        CancellationToken cancellationToken)
    {

        var storeId = StoreId.Create(request.StoreId);
        if (storeId.IsFailure)
            return storeId.Error;

        var store = await storeRepository.GetById(storeId.Value);

        if (store is null)
            return Error.Create("store_not_found", "Store not found");

        var productId = ProductId.Create(request.ProductId);

        if (productId.IsFailure)
            return productId.Error;

        var product = await productRepository.GetByIdAsync(productId.Value, cancellationToken);

        if (product is null)
            return Error.Create("product_not_found", "Product not found");

        var inventoryExists = await inventoryRepository.GetByStoreAndProductId(
            store.Id,
            product.Id,
            cancellationToken);

        var inventoryPrice = Price.Create(request.Price, "IRR");


        if (inventoryPrice.IsFailure)
            return inventoryPrice.Error;

        if (inventoryExists is not null)
        {
            inventoryExists.SetPrice(inventoryPrice.Value);

            var stockResult = inventoryExists.IncreaseStock(request.InitialQuantity);

            if (stockResult.IsFailure)
                return stockResult.Error;

            inventoryRepository.Update(inventoryExists);
        }
        else
        {
            var newInventory = InventoryEntity.Create(productId.Value, storeId.Value, request.InitialQuantity,
                inventoryPrice.Value);

            if (newInventory.IsFailure)
                return newInventory.Error;

            await inventoryRepository.AddAsync(newInventory.Value, cancellationToken);
        }

        if (product.Status == ProductStatus.Draft)
            product.SetStatus(ProductStatus.Available);

        /* // 7️ Publish integration event
         var inventoryCreatedEvent = new InventoryCreatedIntegrationEvent
         {
             Id = Guid.NewGuid(),
             InventoryId = inventory.Id,
             ProductId = product.Id,
             StoreId = store.Id,
             Price = inventory.Price.Amount,
             Quantity = inventory.AvailableQuantity
         };

         await eventPublisher.PublishAsync(
             inventoryCreatedEvent,
             cancellationToken);

         // 8️⃣ Commit transaction*/
        await unitOfWork.CommitAsync(cancellationToken);

        return UnitResult.Success<Error>();
    }
}
