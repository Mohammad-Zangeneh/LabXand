using LabXand.SharedKernel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LabXand.Data.EF;

public class ManyToManyNavigationPropertyUpdate<TRoot, T, I>(INavigationPropertyUpdater<TRoot> root, Expression<Func<TRoot, ICollection<T>>> propertyAccessor) : ListNavigationPropertyUpdater<TRoot, T, I>(root, propertyAccessor)
        where TRoot : class
    where T : class, IEntity<I>
    where I : struct
{
    protected override void OnAdd(DbContext dbContext, ICollection<T> originalPropertyValue, T currentValue) => originalPropertyValue.Add(currentValue);
    protected override void OnDelete(DbContext dbContext, ICollection<T> originalPropertyValue, T originalValue) => originalPropertyValue.Remove(originalValue);
}
