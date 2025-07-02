using CSharpFunctionalExtensions;
using Locamart.Application.Contracts.UseCases.StoreCategory;
using Locamart.Domain.Entities.StoreCategory;
using Locamart.Domain.Entities.StoreCategory.Abstracts;
using Locamart.Domain.Entities.StoreCategory.ValueObjects;
using Locamart.Shared;
using Locamart.Shared.Abstracts;
using Locamart.Shared.Infrastructure;

namespace Locamart.Application.UseCases.StoreCategory.AddStoreCategory;

public class AddStoreCategoryCommandHandler(IStoreCategoryRepository storeCategoryRepository, IUnitOfWork unitOfWork) : ICommandHandler<AddStoreCategoryCommand, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(AddStoreCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var parentId = request.ParentId.HasValue ? StoreCategoryId.Create(request.ParentId.Value) : null;

            var entity = StoreCategoryEntity.Create(request.Name, parentId);

            if (entity.IsFailure)
                return UnitResult.Failure<Error>(entity.Error);

            storeCategoryRepository.Add(entity.Value);

            await unitOfWork.CommitAsync(cancellationToken);

            return UnitResult.Success<Error>();
        }

        catch (Exception ex)
        {
            return UnitResult.Failure<Error>(Error.Create("add_store_category_failed", ex.Message));
        }
    }
}

