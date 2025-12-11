namespace Locamart.Nava.Adapter.Http.User.RequestModels;

public class VerifyOtpRequestModel
{
    public string MobileNumber { get; set; }
    public string ChallengeId { get; set; }
    public string OtpCode { get; set; }
}

