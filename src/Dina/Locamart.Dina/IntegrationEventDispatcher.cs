using Locamart.Dina.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Locamart.Dina;

public class IntegrationEventDispatcher(IServiceScopeFactory scopeFactory) : IIntegrationEventDispatcher
{
    public async Task DispatchAsync<TEvent>(TEvent @event, CancellationToken ct = default)
        where TEvent : IIntegrationEvent
    {
        using var scope = scopeFactory.CreateScope();
        var handlers = scope.ServiceProvider.GetServices<IIntegrationEventHandler<TEvent>>();

        foreach (var h in handlers)
            await h.HandleAsync(@event, ct);
    }
}