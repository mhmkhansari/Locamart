using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Liam.Application.Contracts.Dtos.User;
using Locamart.Liam.Application.Contracts.Services;
using Locamart.Liam.Application.Contracts.UseCases.VerifyOtp;
using Locamart.Liam.Domain;
using Locamart.Liam.Domain.ValueObjects;
using System.Text.Json;

namespace Locamart.Liam.Application.UseCases.VerifyOtp;

public class VerifyOtpCommandHandler(ICacheService cacheService) : ICommandHandler<VerifyOtpCommand, Result<UserDto, Error>>
{
    public async Task<Result<UserDto, Error>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        var jsonTempUser = await cacheService.GetAsync($"temp_user:{request.UserId}");

        if (jsonTempUser.IsFailure)
            return jsonTempUser.Error;

        if (jsonTempUser.Value is null)
            return Error.Create("temp_user_not_found", "Temp user not found");

        var tempUser = JsonSerializer.Deserialize<TempUser>(jsonTempUser.Value);

        if (tempUser?.OtpCode != request.OtpCode || DateTime.UtcNow > tempUser.CreatedAt.AddMinutes(2))
            return Error.Create("invalid_otp_code", "Invalid otp code");

        var mobileNumber = MobileNumber.Create(tempUser.MobileNumber);

        if (mobileNumber.IsFailure)
            return mobileNumber.Error;

        var user = new UserDto()
        {
            Id = tempUser.Id,
            MobileNumber = mobileNumber.Value.Value
        };

        return user;
    }
}

