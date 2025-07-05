using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Liam.Application.Contracts.Dtos.User;
using Locamart.Liam.Application.Contracts.Services;
using Locamart.Liam.Domain;
using System.Text.Json;

namespace Locamart.Liam.Application.Services;

public class LiamIdentityService(ICacheService cacheService) : ILiamIdentityService
{
    public async Task<Result<(string OtpCode, Guid TempUserId), Error>> GenerateAndSendOtp(string mobileNumber)
    {
        try
        {
            var otpCode = new Random().Next(100000, 999999).ToString();
            var tempUserId = Guid.NewGuid();

            var tempUser = new TempUser
            {
                Id = tempUserId,
                MobileNumber = mobileNumber,
                OtpCode = otpCode,
                CreatedAt = DateTime.UtcNow
            };
            await cacheService.SetAsync($"temp_user:{tempUserId}", JsonSerializer.Serialize(tempUser),
                TimeSpan.FromMinutes(2));

            var otpMessage = new OtpMessage
            {
                TempUserId = tempUserId,
                MobileNumber = mobileNumber,
                OtpCode = otpCode,
                CreatedAt = DateTime.UtcNow
            };

            await cacheService.PushToListAsync("otp_queue", JsonSerializer.Serialize(otpMessage));

            return (otpCode, tempUserId);
        }

        catch (Exception ex)
        {
            return Error.Create("send_otp_failed", ex.Message);
        }
    }

    public async Task<Result<TempUserDto, Error>> VerifyOtpAndGetTempUser(Guid tempUserId, string otpCode)
    {
        var jsonTempUser = await cacheService.GetAsync($"temp_user:{tempUserId}");

        if (jsonTempUser.IsFailure)
            return jsonTempUser.Error;

        if (jsonTempUser.Value is null)
            return Error.Create("temp_user_not_found", "Temp user not found");

        var tempUser = JsonSerializer.Deserialize<TempUser>(jsonTempUser.Value);

        if (tempUser?.OtpCode != otpCode || DateTime.UtcNow > tempUser.CreatedAt.AddMinutes(2))
            return Error.Create("invalid_otp_code", "Invalid otp code");

        return new TempUserDto
        {
            Id = tempUser.Id,
            MobileNumber = tempUser.MobileNumber
        };
    }

    public Task<UnitResult<Error>> DeleteTempUser(Guid tempUserId)
    {
        throw new NotImplementedException();
    }

}

