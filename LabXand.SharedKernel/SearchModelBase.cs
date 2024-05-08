using LabXand.Core;
using System.Linq.Expressions;

namespace LabXand.SharedKernel;

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

    public Criteria GetCriteria()
    {
        Criteria criteria = CriteriaBuilder.CreateNew<TEntity>();
        GetSearchMap().ForEach(m =>
        {
            var filterValue = TypeHelper.GetPropertyValue(this, m.PropertyName);
            if (filterValue is null)
                return;
            criteria.And(CriteriaBuilder.CreateFromFilterOperation<TEntity>(m.operation, m.DomainProperty, filterValue));
        });
        return criteria;
    }
}
