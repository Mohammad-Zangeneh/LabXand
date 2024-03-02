namespace LabXand.SharedKernel
{
    public interface IRepository<TAgregateRoot>
        where TAgregateRoot : IAgregateRoot 
    {
        IQueryable<TAgregateRoot> Query { get; }
        void Add(TAgregateRoot domain);
        void Edit(TAgregateRoot domain);
        void Remove(TAgregateRoot domain);
    }
}