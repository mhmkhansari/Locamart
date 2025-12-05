using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Dina.Extensions;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Adapter.Postgresql;
using Locamart.Nava.Application.Contracts.IntegrationEvents;
using Locamart.Nava.Application.Contracts.Services;
using Locamart.Nava.Application.Contracts.UseCases.Store;
using Locamart.Nava.Domain.Entities.Store.Abstracts;
using Locamart.Nava.Domain.Entities.Store.Builders;
using Locamart.Nava.Domain.Entities.StoreCategory.ValueObjects;
using Locamart.Shared.ValueObjects;
using MassTransit;
using Serilog;

namespace Locamart.Nava.Application.UseCases.Store.AddStore;

public class AddStoreCommandHandler(IStoreRepository storeRepository,
    IUnitOfWork unitOfWork,
    IIntegrationEventPublisher eventPublisher,
    ICurrentUser currentUser,
    ILogger logger,
    LocamartNavaDbContext dbContext,
    IPublishEndpoint bus) : ICommandHandler<AddStoreCommand, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(AddStoreCommand request, CancellationToken cancellationToken)
    {
        try
        {
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
                    (b, lat, lon) => b.WithLocation(new Location(lat!.Value, lon!.Value)));

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

            await eventPublisher.PublishAsync(storeCreatedIntegrationEvent, cancellationToken);

            // 1) Log DbContext instance identity you are inspecting
            logger.Information("Handler DbContext instance hash: {hash}", dbContext.GetHashCode());

            // 2) Dump tracked entities (types & states)
            var tracked = dbContext.ChangeTracker.Entries()
                .Select(e => new { Type = e.Entity.GetType().FullName, Name = e.Entity.GetType().Name, State = e.State.ToString() })
                .ToList();

            logger.Information("ChangeTracker after Publish contains {count} entries: {@tracked}", tracked.Count, tracked);

            // 3) Search ChangeTracker explicitly for 'Outbox'/'Inbox'
            var outboxEntries = dbContext.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType().Name.Contains("Outbox") || e.Entity.GetType().Name.Contains("Inbox"))
                .Select(e => new { Type = e.Entity.GetType().FullName, State = e.State.ToString() })
                .ToList();

            logger.Information("Outbox-like entries after Publish: {count}. Details: {@outboxEntries}", outboxEntries.Count, outboxEntries);

            // 4) Log publish endpoint implementation type (inside your MassTransitIntegrationEventPublisher add logging of the bus type)
            logger.Information("PublishEndpoint type: {type}", bus.GetType().FullName);

            await unitOfWork.CommitAsync(cancellationToken);

            return UnitResult.Success<Error>();
        }

        catch (Exception ex)
        {
            return Error.Create("add_store_failed", ex.Message);
        }
    }
}

