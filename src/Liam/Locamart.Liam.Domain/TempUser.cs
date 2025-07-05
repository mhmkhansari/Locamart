namespace Locamart.Liam.Domain;

public class TempUser
{
    public Guid Id { get; set; }
    public string MobileNumber { get; set; }
    public string OtpCode { get; set; }
    public DateTime CreatedAt { get; set; }
}

