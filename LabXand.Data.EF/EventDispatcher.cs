using LabXand.SharedKernel;
using Microsoft.Extensions.DependencyInjection;

namespace LabXand.Data.EF;

public class EventDispatcher(IServiceProvider serviceProvider) : IEventDispatcher
{
    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var eventHandlers = serviceProvider.GetServices<IEventHandler<IDomainEvent>>();

        foreach (var handler in eventHandlers)
        {
            if (handler.CanHandle(domainEvent))
            {
                await handler.HandleAsync(domainEvent, cancellationToken);
            }
        }
    }
}