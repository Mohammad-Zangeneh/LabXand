namespace LabXand.SharedKernel;

public abstract class EventHandlerBase<TDomainEvent> : IEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public abstract bool CanHandle(IDomainEvent domainEvent);

    public abstract Task HandleAsync(TDomainEvent domainEvent, CancellationToken cancellationToken = default);

    public Task HandleAsync(object domainEvent, CancellationToken cancellationToken = default) => HandleAsync((TDomainEvent)domainEvent, cancellationToken);
    
}
