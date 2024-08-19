using LabXand.SharedKernel;
using Microsoft.Extensions.DependencyInjection;

namespace LabXand.Data.EF;

public class EventDispatcher(IServiceProvider serviceProvider) : IEventDispatcher
{
    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IEventHandler<>).MakeGenericType(domainEvent.GetType());
        var handlers = serviceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            var canHandle = handlerType
              .GetMethod(nameof(IEventHandler<IDomainEvent>.CanHandle))?
              .Invoke(handler, [domainEvent]);

            if (canHandle != null && Convert.ToBoolean(canHandle))
            {
                var handleAsyncMethod = handlerType.GetMethod(nameof(IEventHandler<IDomainEvent>.HandleAsync));
                if (handleAsyncMethod != null)
                {
                    await (Task)handleAsyncMethod.Invoke(handler, [domainEvent, cancellationToken]);
                }
            }
        }
    }
}