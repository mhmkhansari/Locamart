using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Liam.Application.Contracts.Services;
using Locamart.Liam.Application.Contracts.UseCases.VerifyOtp;

namespace Locamart.Liam.Application.UseCases.VerifyOtp;

public class VerifyOtpCommandHandler(ILiamIdentityService identityService) : ICommandHandler<VerifyOtpCommand, Result<string, Error>>
{
    public async Task<Result<string, Error>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

