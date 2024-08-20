using LabXand.SharedKernel;
using Microsoft.Extensions.DependencyInjection;

namespace LabXand.Data.EF;

public class EventDispatcher(IServiceProvider serviceProvider) : IEventDispatcher
{
    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IEventHandler<>).MakeGenericType(domainEvent.GetType());
        var handlers = serviceProvider.GetServices(handlerType).Cast<IEventHandler>();

        foreach (var handler in handlers)
        {
            if (handler.CanHandle(domainEvent))
            {
                await handler.HandleAsync(domainEvent, cancellationToken);
            }
        }
    }
}