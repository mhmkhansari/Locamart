using Locamart.Dina.ValueObjects;
using Locamart.Nava.Application.Contracts.Dtos.Cart;

namespace Locamart.Nava.Application.Contracts.Services;

public interface ICartQueryService
{
    Task<UserCartsDto> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken);
}

