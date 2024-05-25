using Ardalis.GuardClauses;
using LabXand.SharedKernel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LabXand.Data.EF;

public abstract class ListNavigationPropertyUpdater<TRoot, T, I>(INavigationPropertyUpdater<TRoot> root, Expression<Func<TRoot, ICollection<T>>> propertyAccessor) : NavigationPropertyUpdaterBase<TRoot, T>(root)
    where TRoot : class
    where T : class, IEntity<I>
    where I : struct
{
    public override string Key => ((MemberExpression)propertyAccessor.Body).Member.Name;
    private readonly Expression<Func<TRoot, ICollection<T>>> propertyAccessor = propertyAccessor;
    protected ICollection<T> GetList(TRoot parent)
    {
        Guard.Against.Null(propertyAccessor);
        Guard.Against.Null(parent, nameof(parent));
        var value = propertyAccessor.Compile().Invoke(parent);
        if (value != null)
            return value;
        return [];
    }
    public override void Update(DbContext dbContext, TRoot rootCurrentValue, TRoot rootOriginalValue)
    {
        ICollection<T> originalNavigationPropertyValue = GetList(rootOriginalValue);
        T[] itterationArray = new T[originalNavigationPropertyValue.Count];
        originalNavigationPropertyValue.CopyTo(itterationArray, 0);
        ICollection<T> currentNavigationPropertyValue = GetList(rootCurrentValue);
        UpdateEntities(dbContext, originalNavigationPropertyValue, currentNavigationPropertyValue);
        DeleteEntities(dbContext, rootOriginalValue, originalNavigationPropertyValue, currentNavigationPropertyValue);
        AddEntities(dbContext, rootCurrentValue, originalNavigationPropertyValue, currentNavigationPropertyValue);
    }

    void UpdateEntities(DbContext dbContext, ICollection<T> originalNavigationPropertyValue, ICollection<T> currentNavigationPropertyValue)
    {
        List<I> mustUpdate = currentNavigationPropertyValue.Join(originalNavigationPropertyValue, c => c.Id, c => c.Id, (c, o) => c.Id).ToList();
        foreach (var id in mustUpdate)
        {
            var currentValue = currentNavigationPropertyValue.First(o => o.Id.Equals(id));
            var originalValue = originalNavigationPropertyValue.First(p => p.Id.Equals(id));
            OnUpdate(dbContext, currentValue, originalValue, id);
        }
    }

    void DeleteEntities(DbContext dbContext, TRoot rootOriginalValue, ICollection<T> originalNavigationPropertyValue, ICollection<T> currentNavigationPropertyValue)
    {
        List<I> mustBeDelte = originalNavigationPropertyValue.Select(o => o.Id).Except(currentNavigationPropertyValue.Select(c => c.Id)).ToList();
        foreach (var id in mustBeDelte)
        {
            var originalValue = originalNavigationPropertyValue.First(o => o.Id.Equals(id));
            navigationPropertyUpdaterCustomizer?.OnBeforRemoveEntity(rootOriginalValue, originalValue).Invoke(rootOriginalValue, originalValue); ;
            OnDelete(dbContext, originalNavigationPropertyValue, originalValue);
            navigationPropertyUpdaterCustomizer?.OnAfterRemoveEntity(rootOriginalValue, originalValue).Invoke(rootOriginalValue, originalValue); ;
        }
    }

    void AddEntities(DbContext dbContext, TRoot rootCurrentValue, ICollection<T> originalNavigationPropertyValue, ICollection<T> currentNavigationPropertyValue)
    {
        List<I> mustBeAdd = currentNavigationPropertyValue.Select(o => o.Id).Except(originalNavigationPropertyValue.Select(c => c.Id)).ToList();
        dbContext.Set<T>().Where(t => mustBeAdd.Contains(t.Id)).ToList();
        foreach (var id in mustBeAdd)
        {
            var currentValue = currentNavigationPropertyValue.First(o => o.Id.Equals(id));
            navigationPropertyUpdaterCustomizer?.OnAfterAddEntity(rootCurrentValue, currentValue).Invoke(rootCurrentValue, currentValue); ;
            OnAdd(dbContext, originalNavigationPropertyValue, currentValue);
            navigationPropertyUpdaterCustomizer?.OnAfterAddEntity(rootCurrentValue, currentValue).Invoke(rootCurrentValue, currentValue); ;
        }
    }

    protected virtual void OnUpdate(DbContext dbContext, T originalValue, T currentValue, I id)
    {

        dbContext.SafeUpdate(currentValue, originalValue);
        foreach (var childUpdater in navigationPropertyUpdaters)
            childUpdater.Update(dbContext, currentValue, originalValue);
    }

    protected abstract void OnDelete(DbContext dbContext, ICollection<T> originalPropertyValue, T originalValue);
    protected abstract void OnAdd(DbContext dbContext, ICollection<T> originalPropertyValue, T currentValue);
}
