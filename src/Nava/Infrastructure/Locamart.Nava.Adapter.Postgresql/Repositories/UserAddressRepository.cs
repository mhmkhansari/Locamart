using Locamart.Dina.ValueObjects;
using Locamart.Nava.Domain.Entities.UserAddress;
using Locamart.Nava.Domain.Entities.UserAddress.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Nava.Adapter.Postgresql.Repositories;

public class UserAddressRepository(LocamartNavaDbContext dbContext) : IUserAddressRepository
{
    public async Task AddAsync(UserAddressEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.UserAddresses.AddAsync(entity, cancellationToken);
    }

    public async Task<IEnumerable<UserAddressEntity>> GetByUserId(UserId userId, CancellationToken cancellationToken)
    {
        return await dbContext.UserAddresses
            .Where(x => x.UserId.Value == userId.Value)
            .ToListAsync(cancellationToken);
    }
}

