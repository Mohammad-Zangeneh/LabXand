using LabXand.Core;
using LabXand.Extensions;
using System.Linq.Expressions;

namespace LabXand.SharedKernel.Filters;

public abstract class SearchModelBase<TEntity, TId> : ISearchModel
    where TEntity : class, IEntity<TId>
    where TId : struct
{
    public SearchModelBase() => ConfigureMapping();
    protected List<(string PropertyName, FilterOperations operation, string DomainProperty)> filterConstructionMap = [];
    protected abstract void ConfigureMapping();
    public List<(string PropertyName, FilterOperations operation, string DomainProperty)> GetSearchMap() => filterConstructionMap;

    public Criteria GetCriteria()
    {
        Criteria criteria = CriteriaBuilder.CreateNew<TEntity>();
        GetSearchMap().ForEach(m =>
        {
            var filterValue = TypeHelper.GetPropertyValue(this, m.PropertyName);
            if (filterValue is null)
                return;
            criteria = criteria.And(CriteriaBuilder.CreateFromFilterOperation<TEntity>(m.operation, m.DomainProperty, filterValue));
        });
        return criteria;
    }

    public void AddMapConfig(string propertyName, FilterOperations operation, string domainProperty) =>
        filterConstructionMap.Add(new(propertyName, operation, domainProperty));

}

public static class SearchModelExtender
{
    public static TSearchModel Register<TSearchModel, TPropertyType, TEntity, TId>(
        this TSearchModel searchModel,
        Expression<Func<TSearchModel, TPropertyType>> searchModelProperty, 
        FilterOperations operation, 
        Expression<Func<TEntity, TPropertyType>> domainProperty)
            where TSearchModel : SearchModelBase<TEntity, TId>
            where TEntity : class, IEntity<TId>
            where TId : struct
    {
        searchModel.AddMapConfig(ExpressionHelper.GetPropertyName(searchModelProperty), operation, ExpressionHelper.GetPropertyName(domainProperty));
        return searchModel;
    }

    public static TSearchModel Register<TSearchModel, TSearchPropertyType, TDomainPropertyType, TEntity, TId>(
        this TSearchModel searchModel,
        Expression<Func<TSearchModel, TSearchPropertyType>> searchModelProperty,
        FilterOperations operation,
        Expression<Func<TEntity, TDomainPropertyType>> domainProperty)
            where TSearchModel : SearchModelBase<TEntity, TId>
            where TEntity : class, IEntity<TId>
            where TId : struct
    {
        searchModel.AddMapConfig(ExpressionHelper.GetPropertyName(searchModelProperty), operation, ExpressionHelper.GetPropertyName(domainProperty));
        return searchModel;
    }
}
