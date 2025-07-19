namespace Locamart.Liam.Application.Contracts.Dtos.User;

public class LoginChallengeModel
{
    public string ChallengeId { get; set; }
    public string MobileNumber { get; set; }
    public string OtpCode { get; set; }
    public DateTime CreatedAt { get; set; }
}
