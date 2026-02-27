using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Dina.Extensions;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Application.Contracts.IntegrationEvents;
using Locamart.Nava.Application.Contracts.Services;
using Locamart.Nava.Application.Contracts.UseCases.Store;
using Locamart.Nava.Domain.Entities.Store.Abstracts;
using Locamart.Nava.Domain.Entities.Store.Builders;
using Locamart.Nava.Domain.Entities.StoreCategory.ValueObjects;
using Serilog;

namespace Locamart.Nava.Application.UseCases.Store.AddStore;

public class AddStoreCommandHandler(IStoreRepository storeRepository,
    IUnitOfWork unitOfWork,
    IIntegrationEventPublisher eventPublisher,
    ICurrentUser currentUser,
    IUserStore userStore,
    ILogger logger) : ICommandHandler<AddStoreCommand, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(AddStoreCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userHasStore = await storeRepository.GetByUserId(currentUser.UserId);

            if (userHasStore is not null)
                return Error.Create("user_has_store", "User already has store");

            var ownerId = UserId.Create(request.CreatedBy);
            if (ownerId.IsFailure)
                return ownerId.Error;

            var storeCategoryId = StoreCategoryId.Create(request.CategoryId);
            if (storeCategoryId.IsFailure)
                return storeCategoryId.Error;

            var builder = new StoreBuilder(request.Name, storeCategoryId.Value, ownerId.Value)
                .MaybeDo(request.Bio, (b, v) => b.WithBio(v))
                .MaybeDo(request.ProfileImage, (b, v) => b.WithProfileImage(new Image(request.ProfileImage!)))
                .MaybeDo(request.Latitude, request.Longitude, (lat, lon) => lat is not null && lon is not null,
                    (b, lat, lon) => b.WithLocation(new GeoLocation(lat!.Value, lon!.Value)));

            var entity = builder.Build();

            if (entity.IsFailure)
                return entity.Error;

            storeRepository.Add(entity.Value);

            var storeCreatedIntegrationEvent = new StoreCreatedIntegrationEvent
            {
                Id = Guid.NewGuid(),
                StoreId = entity.Value.Id,
                OccurredAt = DateTime.UtcNow,
                OwnerId = currentUser.UserId.Value
            };

            var addClaimResult = await userStore.AddClaimAsync(currentUser.UserId.Value, "store-admin", entity.Value.Id.ToString());

            if (addClaimResult.IsFailure)
                return Error.Create("add_store_failed", addClaimResult.Error.Message);

            await eventPublisher.PublishAsync(storeCreatedIntegrationEvent, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);

            return UnitResult.Success<Error>();
        }

        catch (Exception ex)
        {
            return Error.Create("add_store_failed", ex.Message);
        }
    }
}

