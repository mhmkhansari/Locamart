using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Dina.Utils;
using Locamart.Liam.Application.Contracts.Dtos;
using Locamart.Liam.Application.Contracts.Dtos.User;
using Locamart.Liam.Application.Contracts.Services;
using Locamart.Liam.Application.Contracts.UseCases.Login;
using Locamart.Liam.Domain.ValueObjects;
using System.Text.Json;

namespace Locamart.Liam.Application.UseCases.Login;

public class RegisterUserCommandHandler(ICacheService cacheService) : ICommandHandler<LoginCommand, Result<LoginCommandResult, Error>>
{
    public async Task<Result<LoginCommandResult, Error>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var mobileNumber = MobileNumber.Create(request.MobileNumber);

        if (mobileNumber.IsFailure)
            return mobileNumber.Error;

        var otpCode = OtpGenerator.GenerateOtp();

        var challengeId = DinaRandomNumber.GenerateChallengeId();

        var existingChallenge = await cacheService.GetAsync($"login_challenge:{request.MobileNumber}");

        if (existingChallenge.IsFailure)
            return existingChallenge.Error;

        if (existingChallenge.Value is not null)
            return Error.Create("otp_code_already_sent", "Otp code already sent to the number");

        var loginChallenge = new LoginChallengeModel()
        {
            ChallengeId = challengeId,
            MobileNumber = request.MobileNumber,
            OtpCode = otpCode,
            CreatedAt = DateTime.UtcNow
        };

        var challengeCreationResult = await cacheService.SetAsync($"login_challenge:{request.MobileNumber}", JsonSerializer.Serialize(loginChallenge),
            TimeSpan.FromMinutes(2));

        if (challengeCreationResult.IsFailure)
            return challengeCreationResult.Error;

        var otpMessage = new OtpMessageModel
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

