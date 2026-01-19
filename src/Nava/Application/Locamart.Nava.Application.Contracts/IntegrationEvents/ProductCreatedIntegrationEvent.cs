using Locamart.Dina.Abstracts;
using MediatR;

namespace Locamart.Nava.Application.Contracts.IntegrationEvents;

public record ProductCreatedIntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; }
    public DateTime OccurredAt { get; init; }
    public Guid ProductId { get; init; }
    public string ProductName { get; init; }
    public string Description { get; init; }
    public List<string> Images { get; init; }
    public Guid StoreId { get; init; }
    public string StoreUniqueIdentity { get; init; }
    public string StoreName { get; init; }
    public decimal Price { get; init; }
    public double StoreLatitude { get; init; }
    public double StoreLongitude { get; init; }
}
