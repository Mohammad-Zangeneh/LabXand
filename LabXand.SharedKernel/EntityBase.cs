namespace LabXand.SharedKernel
{
    public class EntityBase<TIdentifier> : IEntity<TIdentifier>
        where TIdentifier : struct
    {
        readonly IList<IDomainEvent> events = [];
        public TIdentifier Id { get; protected set; }
        protected void AddDomainEvent(IDomainEvent domainEvent) => events.Add(domainEvent);
        public IList<IDomainEvent> Events => events;
    }
}