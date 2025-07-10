using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Liam.Application.Contracts.Dtos.User;

namespace Locamart.Liam.Application.Contracts.UseCases.VerifyOtp;

public class VerifyOtpCommand : ICommand<Result<UserDto, Error>>
{
    public Guid UserId { get; set; }
    public string OtpCode { get; set; }
}

