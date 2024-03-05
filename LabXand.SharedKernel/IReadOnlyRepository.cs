namespace LabXand.SharedKernel
{
    public interface IReadOnlyRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        IQueryable<TAggregateRoot> Query { get; }
        Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken cancellationToken) where T : class;
        Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken) where T : class;
        Task<List<T>> GetPaginatedItemsAsync<T>(IQueryable<T> query, int page, int size, CancellationToken cancellationToken) where T : class;
        Task<List<T>> GetListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken) where T : class;
    }
}