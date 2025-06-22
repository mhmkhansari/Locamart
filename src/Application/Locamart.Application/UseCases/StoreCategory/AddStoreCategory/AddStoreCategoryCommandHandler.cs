using CSharpFunctionalExtensions;
using Locamart.Application.Contracts.UseCases.StoreCategory;
using Locamart.Domain.StoreCategory;
using Locamart.Domain.StoreCategory.Abstracts;
using Locamart.Domain.StoreCategory.ValueObjects;
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
            var parentId = request.ParentId is not null ? StoreCategoryId.Create(request.ParentId) : null;
            var entity = StoreCategoryEntity.Create(request.Name, parentId);

            storeCategoryRepository.Add(entity);

            await unitOfWork.CommitAsync(cancellationToken);

            return UnitResult.Success<Error>();
        }

        catch (Exception ex)
        {
            return UnitResult.Failure<Error>(Error.Create("add_store_category_failed", ex.Message));
        }
    }
}

