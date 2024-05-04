using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LabXand.Data.EF;

public class OneToOneNavigationPropertyUpdater<TRoot, T>(INavigationPropertyUpdater<TRoot> root, Expression<Func<TRoot, T>> propertyAccessor) : NavigationPropertyUpdaterBase<TRoot, T>(root)
    where TRoot : class
    where T : class
{
    private readonly Expression<Func<TRoot, T>> propertyAccessor = propertyAccessor;
    public override string Key => ((MemberExpression)propertyAccessor.Body).Member.Name;
    protected T? Get(TRoot parent)
    {
        Guard.Against.Null(propertyAccessor);
        Guard.Against.Null(parent, nameof(parent));

        var value = propertyAccessor.Compile().Invoke(parent);
        return value;
    }
    public override void Update(DbContext dbContext, TRoot rootCurrentInstance, TRoot rootOriginalInstance)
    {
        T? current = Get(rootCurrentInstance);
        T? original = Get(rootOriginalInstance);
        if (current == null && original == null)
            return;

        if (original is null)
        {
            if (current == null) return;
            navigationPropertyUpdaterCustomizer?.OnBeforAddEntity(rootCurrentInstance, current).Invoke(rootOriginalInstance, current);
            dbContext.Entry(current).State = EntityState.Added;
            navigationPropertyUpdaterCustomizer?.OnAfterAddEntity(rootCurrentInstance, current).Invoke(rootOriginalInstance, current);
            return;
        }

        if (current is not null)
        {
            dbContext.SafeUpdate(current, original);
            foreach (var childUpdater in navigationPropertyUpdaters.Distinct().ToList())
                childUpdater.Update(dbContext, current, original);
            return;
        }

        navigationPropertyUpdaterCustomizer?.OnBeforRemoveEntity(rootCurrentInstance, original).Invoke(rootOriginalInstance, original);
        dbContext.Entry(original).State = EntityState.Deleted;
        navigationPropertyUpdaterCustomizer?.OnAfterRemoveEntity(rootCurrentInstance, original).Invoke(rootCurrentInstance, original);
        return;
    }
}
