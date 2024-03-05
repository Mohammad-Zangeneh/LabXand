namespace LabXand.SharedKernel
{
    public interface IRepository<TAggregateRoot> : IReadOnlyRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        void Add(TAggregateRoot domain);
        void Edit(TAggregateRoot domain);
        void Remove(TAggregateRoot domain);
    }
}