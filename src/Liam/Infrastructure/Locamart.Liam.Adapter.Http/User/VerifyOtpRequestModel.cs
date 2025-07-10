using OpenIddict.Abstractions;
using System.Globalization;

namespace Locamart.Liam.Adapter.Http.User;

public class VerifyOtpRequestModel
{
    public Guid UserId { get; set; }
    public string OtpCode { get; set; }
}

