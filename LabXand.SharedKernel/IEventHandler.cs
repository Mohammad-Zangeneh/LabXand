namespace LabXand.SharedKernel;

public interface IEventHandler<TDomainEvent> where TDomainEvent : IDomainEvent
{
    bool CanHandle(IDomainEvent domainEvent);
    Task HandleAsync(TDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
