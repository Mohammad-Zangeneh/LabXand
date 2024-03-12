using System.Linq.Expressions;

namespace LabXand.SharedKernel;

public static class RepositoryExtender
{
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

    public static Task<List<TAggregateRoot>> GetPaginatedItemsAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository,
        Expression<Func<TAggregateRoot, bool>> predicate, int page, int size, CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
        => repository.GetPaginatedItemsAsync(repository.Query, predicate, page, size, cancellationToken);

    public static Task<List<TAggregateRoot>> GetPaginatedItemsAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository, int page, int size, CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
        => repository.GetPaginatedItemsAsync(t => true, page, size, cancellationToken);


    public static Task<List<TAggregateRoot>> GetListAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository,
        Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
        => repository.GetListAsync(repository.Query, predicate, cancellationToken);

    public static Task<List<TAggregateRoot>> GetListAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository, CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
        => repository.GetListAsync(t => true, cancellationToken);

}