namespace Locamart.Liam.Application.Contracts.UseCases.RegisterUser;

public class RegisterUserCommandResult
{
    public Guid TempUserId { get; set; }
    public string Message { get; set; }
}

