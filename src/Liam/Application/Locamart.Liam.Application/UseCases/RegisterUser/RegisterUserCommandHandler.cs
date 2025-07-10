using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Liam.Application.Contracts.Dtos;
using Locamart.Liam.Application.Contracts.Dtos.User;
using Locamart.Liam.Application.Contracts.Services;
using Locamart.Liam.Application.Contracts.UseCases.RegisterUser;
using Locamart.Liam.Domain;
using System.Text.Json;

namespace Locamart.Liam.Application.UseCases.RegisterUser;

public class RegisterUserCommandHandler(ICacheService cacheService) : ICommandHandler<RegisterUserCommand, Result<RegisterUserCommandResult, Error>>
{
    public async Task<Result<RegisterUserCommandResult, Error>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var otpCode = OtpGenerator.GenerateOtp();

        var tempUserId = Guid.NewGuid();


        var tempUser = new TempUserDto()
        {
            Id = tempUserId,
            MobileNumber = request.MobileNumber,
            OtpCode = otpCode,
            CreatedAt = DateTime.UtcNow
        };

        var userCreationResult = await cacheService.SetAsync($"temp_user:{tempUserId}", JsonSerializer.Serialize(tempUser),
            TimeSpan.FromMinutes(2));

        if (userCreationResult.IsFailure)
            return userCreationResult.Error;

        var otpMessage = new OtpMessageDto
        {
            TempUserId = tempUserId,
            MobileNumber = request.MobileNumber,
            OtpCode = otpCode,
            CreatedAt = DateTime.UtcNow,
            Message = "کد ورود لوکامارت: "
        };

        await cacheService.PushToListAsync("otp_queue", JsonSerializer.Serialize(otpMessage));

        return new RegisterUserCommandResult
        {
            TempUserId = tempUserId
        };
    }
}

