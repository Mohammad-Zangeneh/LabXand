namespace LabXand.SharedKernel
{
    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot 
    {
        IQueryable<TAggregateRoot> Query { get; }
        void Add(TAggregateRoot domain);
        void Edit(TAggregateRoot domain);
        void Remove(TAggregateRoot domain);
    }
}