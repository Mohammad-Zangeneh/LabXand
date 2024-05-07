using AutoMapper;
using LabXand.Core;
using LabXand.Extensions;
using System.Linq.Expressions;

namespace LabXand.SharedKernel;
public interface ISearchModel
{
    List<(string PropertyName, FilterOperations operation, string DomainProperty)> GetSearchMap();
}

public abstract class SearchModelBase<TEntity, TId> : ISearchModel
    where TEntity : class, IEntity<TId>
    where TId : struct
{
    protected List<(string PropertyName, FilterOperations operation, string DomainProperty)> filterConstructionMap = [];
    protected abstract void ConfigureMapping();
    protected SearchModelBase<TEntity, TId> Register<TSearchModel>(Expression<Func<TSearchModel, dynamic>> searchModelProperty, FilterOperations operation, Expression<Func<TEntity, dynamic>> domainProperty)
        where TSearchModel : class
    {
        filterConstructionMap.Add(new(GetPropertyName(searchModelProperty), operation, GetPropertyName(domainProperty)));
        return this;
    }

    protected string GetPropertyName<T>(Expression<Func<T, dynamic>> propertyAccessor) => ((MemberExpression)propertyAccessor.Body).Member.Name;

    public List<(string PropertyName, FilterOperations operation, string DomainProperty)> GetSearchMap() => filterConstructionMap;
}

public class SearchSpecification<TSearchModel>
    where TSearchModel : ISearchModel
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public Criteria GetCriteria()
    {

    }
}

public abstract class ApplicationServiceBase<TAggregateRoot, TIdentifier>(IRepository<TAggregateRoot, TIdentifier> repository, IUnitOfWork unitOfWork, IMapper mapper)
    where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
    where TIdentifier : struct
{
    private readonly IRepository<TAggregateRoot, TIdentifier> repository = repository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    protected async Task<PagedList<TResponse>> GetPagedListAsync<TResponse, TSearchModel>(IQueryable<TAggregateRoot> query, SearchSpecification<TSearchModel> searchSpecification, CancellationToken cancellationToken)
        where TSearchModel : ISearchModel
    {
        var expression = ExpressionHelper.CreateFromCriteria<TAggregateRoot>(searchSpecification.GetCriteria());
        
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
public interface IApplicationService<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot
{

}