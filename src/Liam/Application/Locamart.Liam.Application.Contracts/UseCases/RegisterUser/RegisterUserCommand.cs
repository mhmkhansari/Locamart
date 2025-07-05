using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;

namespace Locamart.Liam.Application.Contracts.UseCases.RegisterUser;

public record RegisterUserCommand : ICommand<Result<RegisterUserCommandResult, Error>>
{
    public string MobileNumber { get; init; }
}

