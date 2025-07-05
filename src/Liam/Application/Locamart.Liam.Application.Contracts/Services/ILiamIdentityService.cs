using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Liam.Application.Contracts.Dtos.User;

namespace Locamart.Liam.Application.Contracts.Services;

public interface ILiamIdentityService
{
    public Task<Result<(string OtpCode, Guid TempUserId), Error>> GenerateAndSendOtp(string mobileNumber);
    public Task<Result<TempUserDto, Error>> VerifyOtpAndGetTempUser(Guid tempUserId, string otpCode);
    public Task<UnitResult<Error>> DeleteTempUser(Guid tempUserId);
}

