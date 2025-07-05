namespace Locamart.Liam.Domain;

public class OtpMessage
{
    public Guid TempUserId { get; set; }
    public string MobileNumber { get; set; }
    public string OtpCode { get; set; }
    public DateTime CreatedAt { get; set; }
}

