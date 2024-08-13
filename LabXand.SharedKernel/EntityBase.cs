using LabXand.Core;

namespace LabXand.SharedKernel
{
    public class EntityBase : IEntity
    {
        private readonly IList<IDomainEvent> _domainEvents = [];
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }

    public class EntityBase<TIdentifier> : EntityBase, IEntity<TIdentifier>
        where TIdentifier : struct
    {
        public EntityBase() { }
        public EntityBase(TIdentifier identifier) => Id = identifier;

        public TIdentifier Id { get; protected set; }        

        public virtual string DescribeEntity()
        {
            var description = TypeHelper.GetDescription(this);
            return $"{description} with Id {Id}";
        }
    }
}