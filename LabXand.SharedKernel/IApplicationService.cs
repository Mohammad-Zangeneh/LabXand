using AutoMapper;
using LabXand.Core;
using LabXand.Extensions;

namespace LabXand.SharedKernel;

public abstract class ApplicationServiceBase<TAggregateRoot, TIdentifier>(IRepository<TAggregateRoot, TIdentifier> repository, IMapper mapper)
    where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
    where TIdentifier : struct
{
    private readonly IRepository<TAggregateRoot, TIdentifier> repository = repository;
    private readonly IMapper mapper = mapper;

    protected async Task<PagedList<TResponse>> GetPagedListAsync<TResponse, TSearchModel>(IQueryable<TAggregateRoot> query, SearchSpecification<TSearchModel> searchSpecification, CancellationToken cancellationToken)
        where TSearchModel : ISearchModel
    {
        var expression = ExpressionHelper.CreateFromCriteria<TAggregateRoot>(searchSpecification.GetCriteria());
        query = query.Sort(searchSpecification.SortItems);
        return new PagedList<TResponse>((await repository.GetPaginatedItemsAsync(query,
            expression,
            searchSpecification.PageIndex,
            searchSpecification.PageSize,
            cancellationToken)).Select(mapper.Map<TResponse>),
            searchSpecification.PageIndex,
            searchSpecification.PageSize,
            await repository.CountAsync(query, expression, cancellationToken));
    }
}