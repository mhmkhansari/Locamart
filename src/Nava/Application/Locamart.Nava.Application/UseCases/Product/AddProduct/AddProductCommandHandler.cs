using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.IntegrationEvents;
using Locamart.Nava.Application.Contracts.Services;
using Locamart.Nava.Application.Contracts.UseCases.Product.AddProduct;
using Locamart.Nava.Domain.Entities.Product;
using Locamart.Nava.Domain.Entities.Product.Abstracts;
using Locamart.Nava.Domain.Entities.Product.RequestModels;
using Locamart.Nava.Domain.Entities.Store.Abstracts;
using Locamart.Nava.Domain.Entities.Store.ValueObjects;
using Mapster;

namespace Locamart.Nava.Application.UseCases.Product.AddProduct;

public class AddProductCommandHandler(IProductRepository productRepository,
    IStoreRepository storeRepository,
    IUnitOfWork unitOfWork,
    IIntegrationEventPublisher eventPublisher,
    ICurrentUser currentUser) : ICommandHandler<AddProductCommand, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {

        var createProductRequest = request.Adapt<AddProductRequest>();

        var store = await storeRepository.GetById(StoreId.Create(request.StoreId).Value);

        if (store is null)
            return Error.Create("store_not_found", "Store not found");

        var product = ProductEntity.Create(createProductRequest);

        await productRepository.AddAsync(product.Value, cancellationToken);

        var productCreatedIntegrationEvent = new ProductCreatedIntegrationEvent()
        {
            Id = Guid.NewGuid(),
            OccurredAt = DateTime.UtcNow,
            ProductId = product.Value.Id,
            Name = product.Value.Title,
            Description = product.Value.Description,
            Images = product.Value.Images.Select(x => new ProductImageDto()
            {
                Url = x.Url,
                Order = x.Ordering,
            }).ToArray(),
            IsDeleted = product.Value.IsDeleted,
            CreatedAt = product.Value.CreatedAt,
            CreatedBy = currentUser.UserId

        };

        await eventPublisher.PublishAsync(productCreatedIntegrationEvent, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);

        return UnitResult.Success<Error>();
    }
}

