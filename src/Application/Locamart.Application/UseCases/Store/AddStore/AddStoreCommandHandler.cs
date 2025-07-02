using CSharpFunctionalExtensions;
using Locamart.Application.Contracts.UseCases.Store;
using Locamart.Domain.Entities.Store;
using Locamart.Domain.Entities.Store.Abstracts;
using Locamart.Domain.Entities.StoreCategory.ValueObjects;
using Locamart.Shared;
using Locamart.Shared.Abstracts;
using Locamart.Shared.Extensions;
using Locamart.Shared.Infrastructure;

namespace Locamart.Application.UseCases.Store.AddStore;

public class AddStoreCommandHandler(IStoreRepository storeRepository, IUnitOfWork unitOfWork) : ICommandHandler<AddStoreCommand, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(AddStoreCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var storeCategoryId = StoreCategoryId.Create(request.CategoryId);
            var builder = new StoreBuilder(request.Name, storeCategoryId)
                .MaybeDo(request.Bio, (b, v) => b.WithBio(v))
                .MaybeDo(request.Latitude, request.Longitude, (b, lat, lon) => b.WithLocation())

            builder


            var entity = StoreEntity.Create(request.Name, );

            if (entity.IsFailure)
                return UnitResult.Failure<Error>(entity.Error);

            storeRepository.Add(entity.Value);

            await unitOfWork.CommitAsync(cancellationToken);

            return UnitResult.Success<Error>();
        }

        catch (Exception ex)
        {
            return UnitResult.Failure<Error>(Error.Create("add_store_failed", ex.Message));
        }
    }
}

