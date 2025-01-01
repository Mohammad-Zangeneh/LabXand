using System.Linq.Expressions;

namespace LabXand.SharedKernel;

public interface IReadOnlyRepository<TAggregateRoot, TIdentifier>
    where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
    where TIdentifier : struct
{
    IQueryable<TAggregateRoot> Query { get; }
    IQueryable<TAggregateRoot> GetQuery(bool trackedQuery = false);
    Task<int> CountAsync(IQueryable<TAggregateRoot> query, Expression<Func<TAggregateRoot, bool>> expression, CancellationToken cancellationToken);
    Task<TAggregateRoot?> GetByIdAsync(TIdentifier identifier, CancellationToken cancellationToken);
    Task<TResult?> GetByIdAsync<TResult, TId>(TId identifier, CancellationToken cancellationToken) 
        where TResult : EntityBase<TId>
        where TId : struct;
    Task<TAggregateRoot?> GetAsync(IQueryable<TAggregateRoot> query, Expression<Func<TAggregateRoot, bool>> expression, CancellationToken cancellationToken);
    Task<TResult?> GetAsync<TResult>(IQueryable<TResult> query, CancellationToken cancellationToken)
        where TResult : class;
    Task<List<TAggregateRoot>> GetPaginatedItemsAsync(IQueryable<TAggregateRoot> query, Expression<Func<TAggregateRoot, bool>> expression, int page, int size, CancellationToken cancellationToken);
    Task<List<TResult>> GetPaginatedItemsAsync<TResult>(IQueryable<TResult> query, int page, int size, CancellationToken cancellationToken)
        where TResult : class;
    Task<List<TAggregateRoot>> GetListAsync(IQueryable<TAggregateRoot> query, Expression<Func<TAggregateRoot, bool>> expression, CancellationToken cancellationToken);
    Task<List<TResult>> GetListAsync<TResult>(IQueryable<TResult> query, CancellationToken cancellationToken)
        where TResult : class;
}