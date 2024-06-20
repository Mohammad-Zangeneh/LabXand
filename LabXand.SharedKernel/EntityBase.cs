using LabXand.Core;

namespace LabXand.SharedKernel
{
    public class EntityBase<TIdentifier> : IEntity<TIdentifier>
        where TIdentifier : struct
    {
        public EntityBase() { }
        public EntityBase(TIdentifier identifier) => Id = identifier;
        readonly IList<IDomainEvent> events = [];
        public TIdentifier Id { get; protected set; }
        protected void AddDomainEvent(IDomainEvent domainEvent) => events.Add(domainEvent);

        public virtual string DescribeEntity()
        {
            var description = TypeHelper.GetDescription(this);
            return $"{description} with Id {Id}";
        }
        
        public IList<IDomainEvent> Events => events;
    }
}