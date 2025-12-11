using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;

namespace Locamart.Nava.Application.Contracts.UseCases.User.Login;

public record LoginCommand : ICommand<Result<LoginCommandResult, Error>>
{
    public string MobileNumber { get; init; }
}
