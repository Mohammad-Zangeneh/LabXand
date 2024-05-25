using AutoMapper;
using AutoMapper.QueryableExtensions;
using LabXand.Core;
using LabXand.Extensions;
using LabXand.SharedKernel.Filters;

namespace LabXand.SharedKernel;

public abstract class ApplicationServiceBase<TRepository, TAggregateRoot, TIdentifier>(TRepository repository, IMapper mapper) : IApplicationService<TAggregateRoot>
    where TRepository : IRepository<TAggregateRoot, TIdentifier>
    where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
    where TIdentifier : struct
{
    protected readonly TRepository repository = repository;
    protected readonly IMapper mapper = mapper;

    protected Task<List<TResponse>> GetListAsync<TResponse, TSearchModel>(IQueryable<TAggregateRoot> query, SearchSpecification<TSearchModel> searchSpecification, CancellationToken cancellationToken)
        where TSearchModel : ISearchModel, new()
        where TResponse : class
    {
        var expression = ExpressionHelper.CreateFromCriteria<TAggregateRoot>(searchSpecification.GetCriteria());
        query = query.Where(expression).Sort(searchSpecification.SortItems);
        return repository.GetListAsync(query.ProjectTo<TResponse>(mapper.ConfigurationProvider), cancellationToken);
    }

    protected async Task<PagedList<TResponse>> GetPagedListAsync<TResponse, TSearchModel>(IQueryable<TAggregateRoot> query, 
        PagableSearchSpecification<TSearchModel> searchSpecification, 
        CancellationToken cancellationToken)
        where TSearchModel : ISearchModel, new()
    {
        var expression = ExpressionHelper.CreateFromCriteria<TAggregateRoot>(searchSpecification.GetCriteria());
        query = query.Sort(searchSpecification.SortItems);
        return new PagedList<TResponse>(
            (await repository.GetPaginatedItemsAsync(query, expression, searchSpecification.PageIndex, searchSpecification.PageSize, cancellationToken))
            .Select(mapper.Map<TResponse>),
            searchSpecification.PageIndex,
            searchSpecification.PageSize,
            await repository.CountAsync(query, expression, cancellationToken));
    }

    protected async Task<TResponse> GetAsync<TResponse, TSearchModel>(IQueryable<TAggregateRoot> query, SearchSpecification<TSearchModel> searchSpecification, CancellationToken cancellationToken)
        where TSearchModel : ISearchModel, new()
        where TResponse : class
    {
        var expression = ExpressionHelper.CreateFromCriteria<TAggregateRoot>(searchSpecification.GetCriteria());
        query = query.Where(expression).Sort(searchSpecification.SortItems);
        return mapper.Map<TAggregateRoot, TResponse>(await repository.GetAsync(query, cancellationToken));
    }

    protected Task<int> CountAsync<TSearchModel>(IQueryable<TAggregateRoot> query, SearchSpecification<TSearchModel> searchSpecification, CancellationToken cancellationToken)
        where TSearchModel : ISearchModel, new()
    {
        var expression = ExpressionHelper.CreateFromCriteria<TAggregateRoot>(searchSpecification.GetCriteria());
        query = query.Where(expression).Sort(searchSpecification.SortItems);
        return repository.CountAsync(query, cancellationToken);
    }
}