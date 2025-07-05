using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Liam.Application.Contracts.Services;
using Locamart.Liam.Application.Contracts.UseCases.RegisterUser;

namespace Locamart.Liam.Application.UseCases.RegisterUser;

public class RegisterUserCommandHandler(ILiamIdentityService identityService) : ICommandHandler<RegisterUserCommand, Result<RegisterUserCommandResult, Error>>
{
    public async Task<Result<RegisterUserCommandResult, Error>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = await identityService.GenerateAndSendOtp(request.MobileNumber);

        if (result.IsFailure)
            return result.Error;

        return new RegisterUserCommandResult
        {
            TempUserId = result.Value.TempUserId,
            OtpCode = result.Value.OtpCode
        };
    }
}

