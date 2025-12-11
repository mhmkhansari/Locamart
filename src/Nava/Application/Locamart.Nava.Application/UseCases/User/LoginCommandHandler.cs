using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Dina.Utils;
using Locamart.Nava.Application.Contracts.Dtos.User;
using Locamart.Nava.Application.Contracts.Services;
using Locamart.Nava.Application.Contracts.UseCases.User.Login;
using Locamart.Nava.Domain.Entities.User.ValueObjects;
using System.Text.Json;

namespace Locamart.Nava.Application.UseCases.User;

public class LoginCommandHandler(ICacheService cacheService)
    : ICommandHandler<LoginCommand, Result<LoginCommandResult, Error>>
{
    public async Task<Result<LoginCommandResult, Error>> Handle(LoginCommand request,
        CancellationToken cancellationToken)
    {
        var mobileNumber = MobileNumber.Create(request.MobileNumber);

        if (mobileNumber.IsFailure)
            return mobileNumber.Error;

        var otpCode = DinaRandomNumber.GenerateOtp();

        var challengeId = DinaRandomNumber.GenerateChallengeId();

        var existingChallenge = await cacheService.GetAsync($"login_challenge:{request.MobileNumber}");

        if (existingChallenge.IsFailure)
            return existingChallenge.Error;

        if (existingChallenge.Value is not null)
            return Error.Create("otp_code_already_sent", "Otp code already sent to the number");

        var loginChallenge = new LoginChallengeDto()
        {
            ChallengeId = challengeId,
            MobileNumber = request.MobileNumber,
            OtpCode = otpCode,
            CreatedAt = DateTime.UtcNow
        };

        var challengeCreationResult = await cacheService.SetAsync($"login_challenge:{request.MobileNumber}",
            JsonSerializer.Serialize(loginChallenge),
            TimeSpan.FromMinutes(2));

        if (challengeCreationResult.IsFailure)
            return challengeCreationResult.Error;

        var otpMessage = new OtpMessageDto
        {
            MobileNumber = request.MobileNumber,
            OtpCode = otpCode,
            CreatedAt = DateTime.UtcNow,
            Message = "کد ورود لوکامارت: "
        };
        await cacheService.PushToListAsync("otp_queue", JsonSerializer.Serialize(otpMessage));

        return new LoginCommandResult()
        {
            ChallengeId = challengeId,
            Message = "کد ورود به شماره شما ارسال شد",
            ExpiresIn = 120
        };
    }
}
