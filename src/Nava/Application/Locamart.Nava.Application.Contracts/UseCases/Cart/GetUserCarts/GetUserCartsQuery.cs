using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.Dtos.Cart;

namespace Locamart.Nava.Application.Contracts.UseCases.Cart.GetUserCarts;

public class GetUserCartsQuery : IQuery<Result<UserCartsDto, Error>>
{
    public Guid UserId { get; set; }
}

