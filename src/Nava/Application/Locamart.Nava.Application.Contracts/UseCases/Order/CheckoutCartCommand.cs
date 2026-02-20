using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;

namespace Locamart.Nava.Application.Contracts.UseCases.Order;

public class CheckoutCartCommand : ICommand<Result<CheckoutCartCommandResponse, Error>>
{
    public Guid UserId { get; set; }
    public Guid StoreId { get; set; }
}

