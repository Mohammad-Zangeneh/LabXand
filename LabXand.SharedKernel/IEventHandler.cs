namespace LabXand.SharedKernel;

public interface IEventHandler
{
    bool CanHandle(IDomainEvent domainEvent);
    Task HandleAsync(object domainEvent, CancellationToken cancellationToken = default);
}

public interface IEventHandler<TDomainEvent> : IEventHandler
    where TDomainEvent : IDomainEvent
{
    Task HandleAsync(TDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
