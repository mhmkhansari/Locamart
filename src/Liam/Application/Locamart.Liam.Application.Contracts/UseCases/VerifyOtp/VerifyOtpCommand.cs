using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;

namespace Locamart.Liam.Application.Contracts.UseCases.VerifyOtp;

public class VerifyOtpCommand : ICommand<Result<string, Error>>
{
    public Guid UserId { get; set; }
    public string OtpCode { get; set; }
}

