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
    public static Task<int> CountAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository, IQueryable<TAggregateRoot> query, CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
        => repository.CountAsync(query, t => true, cancellationToken);
    public static Task<int> CountBySpecificationAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository, 
        ISpecification<TAggregateRoot, TIdentifier> specification,
        CancellationToken cancellationToken)
    where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
    where TIdentifier : struct
    => repository.CountAsync(specification.Criteria, cancellationToken);
    public static Task<int> CountBySpecificationAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository, 
        IQueryable<TAggregateRoot> query, 
        ISpecification<TAggregateRoot, TIdentifier> specification,
        CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
        => repository.CountAsync(query, specification.Criteria, cancellationToken);
    #endregion

    #region GetAsync
    public static Task<TAggregateRoot?> GetAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository,
    Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
        => repository.GetAsync(repository.Query, predicate, cancellationToken);
    public static Task<TAggregateRoot?> GetAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository,
        CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
            => repository.GetAsync(t => true, cancellationToken);
    public static Task<TAggregateRoot?> GetAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository,
        IQueryable<TAggregateRoot> query, CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
            => repository.GetAsync(query, t => true, cancellationToken);

    public static Task<TAggregateRoot?> GetBySpecificationAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository,
        ISpecification<TAggregateRoot, TIdentifier> specification,
        CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
            => repository.GetAsync(specification.Criteria, cancellationToken);
    public static Task<TAggregateRoot?> GetBySpecificationAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository,
        ISpecification<TAggregateRoot, TIdentifier> specification,
        IQueryable<TAggregateRoot> query, 
        CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
            => repository.GetAsync(query, specification.Criteria, cancellationToken);

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
    public static Task<List<TAggregateRoot>> GetPaginatedItemsAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository, IQueryable<TAggregateRoot> query,
        int page, int size, CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
    => repository.GetPaginatedItemsAsync(query, t => true, page, size, cancellationToken);
    public static Task<List<TAggregateRoot>> GetPaginatedItemsBySpecificationAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository, 
        ISpecification<TAggregateRoot, TIdentifier> specification,
        int page, int size, CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
        => repository.GetPaginatedItemsAsync(specification.Criteria, page, size, cancellationToken);
    public static Task<List<TAggregateRoot>> GetPaginatedItemsBySpecificationAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository, IQueryable<TAggregateRoot> query,
        ISpecification<TAggregateRoot, TIdentifier> specification,
        int page, int size, CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
        => repository.GetPaginatedItemsAsync(query, specification.Criteria, page, size, cancellationToken);
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
    public static Task<List<TAggregateRoot>> GetListAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository,
        IQueryable<TAggregateRoot> query, CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
    => repository.GetListAsync(query, t => true, cancellationToken);

    public static Task<List<TAggregateRoot>> GetListBySpecificationAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository,
        ISpecification<TAggregateRoot, TIdentifier> specification, CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
        => repository.GetListAsync(repository.Query, specification.Criteria, cancellationToken);

    public static Task<List<TAggregateRoot>> GetListBySpecificationAsync<TAggregateRoot, TIdentifier>(this IRepository<TAggregateRoot, TIdentifier> repository,
        IQueryable<TAggregateRoot> query,
        ISpecification<TAggregateRoot, TIdentifier> specification,
        CancellationToken cancellationToken)
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
        => repository.GetListAsync(query, specification.Criteria, cancellationToken);
    #endregion
}