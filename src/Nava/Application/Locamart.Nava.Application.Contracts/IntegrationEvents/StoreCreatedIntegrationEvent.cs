using MediatR;

namespace Locamart.Nava.Application.Contracts.IntegrationEvents;

public record StoreCreatedIntegrationEvent : INotification
{
    public Guid Id { get; init; }
    public DateTime OccurredAt { get; init; }
    public Guid StoreId { get; init; }
    public Guid OwnerId { get; init; }

}

