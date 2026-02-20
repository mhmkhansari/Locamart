using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Nava.Application.Contracts.UseCases.Order;
using Locamart.Nava.Domain.Entities.Cart.Abstracts;
using Locamart.Nava.Domain.Entities.Order.Abstracts;

namespace Locamart.Nava.Application.UseCases.Order;

public class CheckoutCartCommandHandler(ICartRepository cartRepository,
                                        IOrderRepository orderRepository)
    : ICommandHandler<CheckoutCartCommand, Result<CheckoutCartCommandResponse, Error>>
{
    public Task<Result<CheckoutCartCommandResponse, Error>> Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

