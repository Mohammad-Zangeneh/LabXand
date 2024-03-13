using System.Linq.Expressions;

namespace LabXand.SharedKernel;

public static class RepositoryExtender
{
    #region CountAsync
    public static Task<int> CountAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository,
    Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
    where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
    where TIdentifier : struct
    => repository.CountAsync(repository.Query, predicate, cancellationToken);
    public static Task<int> CountAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository, CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
        => repository.CountAsync(t => true, cancellationToken); 
    #endregion

    #region FirstOrDefaultAsync
    public static Task<TAggregateRoot?> FirstOrDefaultAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository,
    Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
    where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
    where TIdentifier : struct
        => repository.FirstOrDefaultAsync(repository.Query, predicate, cancellationToken);
    public static Task<TAggregateRoot?> FirstOrDefaultAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository,
        CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
            => repository.FirstOrDefaultAsync(t => true, cancellationToken);
    #endregion

    #region GetPaginatedItemsAsync
    public static Task<List<TAggregateRoot>> GetPaginatedItemsAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository,
    Expression<Func<TAggregateRoot, bool>> predicate, int page, int size, CancellationToken cancellationToken)
    where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
    where TIdentifier : struct
    => repository.GetPaginatedItemsAsync(repository.Query, predicate, page, size, cancellationToken);

    public static Task<List<TAggregateRoot>> GetPaginatedItemsAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository, int page, int size, CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
        => repository.GetPaginatedItemsAsync(t => true, page, size, cancellationToken);
    #endregion

    #region GetListAsync
    public static Task<List<TAggregateRoot>> GetListAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository,
   Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
   where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
   where TIdentifier : struct
   => repository.GetListAsync(repository.Query, predicate, cancellationToken);

    public static Task<List<TAggregateRoot>> GetListAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository, CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
        => repository.GetListAsync(t => true, cancellationToken); 
    #endregion

}