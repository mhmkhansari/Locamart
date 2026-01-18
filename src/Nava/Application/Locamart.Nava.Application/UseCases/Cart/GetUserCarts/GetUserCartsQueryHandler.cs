using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Application.Contracts.Dtos.Cart;
using Locamart.Nava.Application.Contracts.Services;
using Locamart.Nava.Application.Contracts.UseCases.Cart.GetUserCarts;
using Locamart.Nava.Domain.Entities.Cart.Abstracts;

namespace Locamart.Nava.Application.UseCases.Cart.GetUserCarts;

public class GetUserCartsQueryHandler(ICartQueryService repository) : IQueryHandler<GetUserCartsQuery, Result<UserCartsDto, Error>>
{
    public async Task<Result<UserCartsDto, Error>> Handle(GetUserCartsQuery request, CancellationToken cancellationToken)
    {
        var userId = UserId.Create(request.UserId);

        if (userId.IsFailure)
            return userId.Error;

        var result = await repository.GetByUserIdAsync(userId.Value, cancellationToken);


        return Result.Success<UserCartsDto, Error>(result);
    }
}

