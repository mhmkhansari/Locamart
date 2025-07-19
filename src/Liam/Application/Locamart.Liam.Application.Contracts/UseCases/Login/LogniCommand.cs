using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;

namespace Locamart.Liam.Application.Contracts.UseCases.Login;

public record LoginCommand : ICommand<Result<LoginCommandResult, Error>>
{
    public string MobileNumber { get; init; }
}

