using Locamart.Shared.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Reflection;

namespace Locamart.Adapter.Postgresql.DomainEvents;

public interface IDomainEventsDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken);
}

internal sealed class DomainEventsDispatcher(IServiceProvider serviceProvider) : IDomainEventsDispatcher
{
    private static readonly ConcurrentDictionary<Type, Type> handlerTypeDictionary = new();
    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (IDomainEvent domainEvent in domainEvents)
        {
            using IServiceScope scope = serviceProvider.CreateScope();

            Type handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());

            IEnumerable<object?> handlers = scope.ServiceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                if (handler is null)
                {
                    continue;
                }

                MethodInfo? handleMethod = handlerType.GetMethod("Handle");

                if (handleMethod is not null)
                {
                    await (Task)handleMethod.Invoke(handler, [domainEvent, cancellationToken]);
                }
            }

        }
    }
}

