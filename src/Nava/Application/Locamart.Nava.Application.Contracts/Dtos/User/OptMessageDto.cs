namespace Locamart.Nava.Application.Contracts.Dtos.User;

public class OtpMessageDto
{
    public string MobileNumber { get; set; }
    public string OtpCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Message { get; set; }
}