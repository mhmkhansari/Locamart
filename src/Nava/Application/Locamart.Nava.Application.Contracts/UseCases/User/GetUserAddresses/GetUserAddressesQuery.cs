using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.Dtos.User;

namespace Locamart.Nava.Application.Contracts.UseCases.User.GetUserAddresses;

public class GetUserAddressesQuery : IQuery<Result<IEnumerable<UserAddressesDto>?, Error>>
{
    public Guid UserId { get; set; }
}

