namespace LabXand.SharedKernel
{
    public interface IRepository<TAgregateRoot, TIdentifier>
        where TIdentifier : struct 
        where TAgregateRoot : IAgregateRoot<TIdentifier> 
    {
        IQueryable<TAgregateRoot> Query { get; }
        void Add(TAgregateRoot domain);
        void Edit(TAgregateRoot domain);
        void Remove(TAgregateRoot domain);
    }
}