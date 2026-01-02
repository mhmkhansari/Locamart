using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Application.Contracts.UseCases.User.AddUserAddress;
using Locamart.Nava.Domain.Entities.UserAddress;
using Locamart.Nava.Domain.Entities.UserAddress.Abstracts;

namespace Locamart.Nava.Application.UseCases.User.AddUserAddress;

public class AddUserAddressCommandHandler(IUserAddressRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<AddUserAddressCommand, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(AddUserAddressCommand request, CancellationToken cancellationToken)
    {

        var userId = UserId.Create(request.UserId);

        if (userId.IsFailure)
            return userId.Error;

        var location = new Location(request.Latitude, request.Longitude);

        var entity = UserAddressEntity.Create(userId.Value, request.Name, location, request.ProvinceId,
            request.CityId,
            request.AddressText, request.PostalCode);

        if (entity.IsFailure)
            return entity.Error;

        await repository.AddAsync(entity.Value, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);

        return UnitResult.Success<Error>();

    }
}

