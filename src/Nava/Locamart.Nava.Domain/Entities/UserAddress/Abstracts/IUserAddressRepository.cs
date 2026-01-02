using Locamart.Dina.ValueObjects;

namespace Locamart.Nava.Domain.Entities.UserAddress.Abstracts;

public interface IUserAddressRepository
{
    Task AddAsync(UserAddressEntity entity, CancellationToken cancellationToken);
    Task<IEnumerable<UserAddressEntity>> GetByUserId(UserId userId, CancellationToken cancellationToken);
}

