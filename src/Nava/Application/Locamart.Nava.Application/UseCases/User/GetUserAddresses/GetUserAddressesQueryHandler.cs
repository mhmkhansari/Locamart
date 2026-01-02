using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Application.Contracts.Dtos.User;
using Locamart.Nava.Application.Contracts.UseCases.User.GetUserAddresses;
using Locamart.Nava.Domain.Entities.UserAddress.Abstracts;

namespace Locamart.Nava.Application.UseCases.User.GetUserAddresses;

public class GetUserAddressesQueryHandler(IUserAddressRepository repository) : IQueryHandler<GetUserAddressesQuery, Result<IEnumerable<UserAddressesDto>?, Error>>
{
    public async Task<Result<IEnumerable<UserAddressesDto>?, Error>> Handle(GetUserAddressesQuery request, CancellationToken cancellationToken)
    {
        var userId = UserId.Create(request.UserId);

        if (userId.IsFailure)
            return userId.Error;

        var result = await repository.GetByUserId(userId.Value, cancellationToken);

        var mappedResult = result.Select(x => new UserAddressesDto()
        {
            Latitude = x.Location.Latitude,
            Longitude = x.Location.Longitude,
            AddressText = x.AddressText,
            PostalCode = x.PostalCode,
            CityId = x.CityId,
            ProvinceId = x.ProvinceId,
            Name = x.Name
        });

        return Result.Success<IEnumerable<UserAddressesDto>?, Error>(mappedResult);
    }
}

