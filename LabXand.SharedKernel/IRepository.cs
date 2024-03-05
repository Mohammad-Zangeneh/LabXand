using LabXand.Core;

namespace LabXand.SharedKernel
{
    public interface IReadOnlyRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        IQueryable<TAggregateRoot> Query { get; }
        Task<IList<T>> GetListAsync<T>(IQueryable<T> query);
        Task<T> GetAsync<T>(IQueryable<T> query);
        Task<IPagedList<T>> GetPageAsync<T>(IQueryable<T> query, int page, int size);

    }
    public interface IRepository<TAggregateRoot> : IReadOnlyRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot 
    {
        void Add(TAggregateRoot domain);
        void Edit(TAggregateRoot domain);
        void Remove(TAggregateRoot domain);
    }
}