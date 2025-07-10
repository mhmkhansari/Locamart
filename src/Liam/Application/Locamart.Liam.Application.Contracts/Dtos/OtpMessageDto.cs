namespace Locamart.Liam.Application.Contracts.Dtos;

public class OtpMessageDto
{
    public Guid TempUserId { get; set; }
    public string MobileNumber { get; set; }
    public string OtpCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Message { get; set; }
}