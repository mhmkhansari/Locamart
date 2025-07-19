using OpenIddict.Abstractions;
using System.Globalization;

namespace Locamart.Liam.Adapter.Http.User;

public class VerifyOtpRequestModel
{
    public string MobileNumber { get; set; }
    public string ChallengeId { get; set; }
    public string OtpCode { get; set; }
}

