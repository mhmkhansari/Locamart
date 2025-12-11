namespace Locamart.Nava.Application.Contracts.Dtos.User;

public class LoginChallengeDto
{
    public string ChallengeId { get; set; }
    public string MobileNumber { get; set; }
    public string OtpCode { get; set; }
    public DateTime CreatedAt { get; set; }
}
