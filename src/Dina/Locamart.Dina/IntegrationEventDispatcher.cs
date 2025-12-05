using Locamart.Dina.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Locamart.Dina;

public class IntegrationEventDispatcher(IServiceProvider provider) : IIntegrationEventDispatcher
{
    public async Task DispatchAsync<TEvent>(TEvent @event, CancellationToken ct = default)
        where TEvent : IIntegrationEvent
    {
        var handlers = provider.GetServices<IIntegrationEventHandler<TEvent>>();

        foreach (var h in handlers)
            await h.HandleAsync(@event, ct);
    }
}