using LabXand.SharedKernel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LabXand.Data.EF;

public class ManyToManyNavigationPropertyUpdater<TRoot, T, I>(INavigationPropertyUpdater<TRoot> root, Expression<Func<TRoot, ICollection<T>>> propertyAccessor) : ListNavigationPropertyUpdater<TRoot, T, I>(root, propertyAccessor)
        where TRoot : class
    where T : class, IEntity<I>
    where I : struct
{
    protected override void AddEntities(DbContext dbContext, TRoot rootCurrentValue, ICollection<T> originalNavigationPropertyValue, ICollection<T> currentNavigationPropertyValue)
    {
        var newValues = currentNavigationPropertyValue.Except(originalNavigationPropertyValue);
        var newValueIds = newValues.Select(r => r.Id);
        var dbValues = dbContext.Set<T>().Where(entity => newValueIds.Contains(entity.Id)).ToList();

        dbValues.ForEach(t => navigationPropertyUpdaterCustomizer?.OnBeforAddEntity(rootCurrentValue, t).Invoke(rootCurrentValue, t));
        dbValues.ForEach(originalNavigationPropertyValue.Add);
        dbValues.ForEach(t => navigationPropertyUpdaterCustomizer?.OnAfterAddEntity(rootCurrentValue, t).Invoke(rootCurrentValue, t));
    }
    protected override void OnDelete(DbContext dbContext, ICollection<T> originalPropertyValue, T originalValue) => originalPropertyValue.Remove(originalValue);
    protected override void UpdateEntities(DbContext dbContext, ICollection<T> originalNavigationPropertyValue, ICollection<T> currentNavigationPropertyValue)
    {
        
    }
}
