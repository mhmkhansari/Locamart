using Locamart.Dina.Abstracts;

namespace Locamart.Nava.Domain.Entities.Product.Events;

public record ProductCreated : IIntegrationEvent
{
    public Guid Id { get; }
    public DateTime OccurredAt { get; }
    public Guid ProductId { get; }
    public double Latitude { get; }
    public double Longitude { get; }
    public string ProductName { get; }
    public string StoreName { get; }
    public decimal Price { get; }
}

