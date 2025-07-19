namespace Locamart.Liam.Application.Contracts.UseCases.Login;

public class LoginCommandResult
{
    public string ChallengeId { get; set; }
    public string Message { get; set; }
    public int ExpiresIn { get; set; }
}

