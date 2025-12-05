using Locamart.Dina.Abstracts;
using MediatR;

namespace Locamart.Nava.Application.Contracts.IntegrationEvents;

public record StoreCreatedIntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; }
    public DateTime OccurredAt { get; init; }
    public Guid StoreId { get; init; }
    public Guid OwnerId { get; init; }

}

