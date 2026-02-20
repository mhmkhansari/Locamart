namespace Locamart.Nava.Application.Contracts.UseCases.Order;

public class CheckoutCartCommandResponse
{
    public Guid OrderId { get; set; }
    public DateTime Ttl { get; set; }
}

