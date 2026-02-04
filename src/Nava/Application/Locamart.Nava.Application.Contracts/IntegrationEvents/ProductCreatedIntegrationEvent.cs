using Locamart.Dina.Abstracts;
using MediatR;

namespace Locamart.Nava.Application.Contracts.IntegrationEvents;

public record ProductCreatedIntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; }
    public DateTime OccurredAt { get; init; }
    public Guid ProductId { get; init; }
    public string Name { get; init; } = default!;
    public string Description { get; init; }
    public IReadOnlyCollection<ProductImageDto> Images { get; init; }
        = [];

    public DateTime CreatedAt { get; init; }
    public Guid CreatedBy { get; init; }
    public bool IsDeleted { get; init; }
}

public record ProductImageDto
{
    public string Url { get; init; } = default!;
    public int Order { get; init; }
}
