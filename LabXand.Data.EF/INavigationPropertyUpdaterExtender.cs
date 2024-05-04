using LabXand.SharedKernel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LabXand.Data.EF;

public static class INavigationPropertyUpdaterExtender
{
    public static INavigationPropertyUpdater<TRoot, T> UpdateOne<TRoot, T>(this INavigationPropertyUpdater<TRoot> propertyUpdater,
        Expression<Func<TRoot, T>> propertySelector)
        where TRoot : class
        where T : class
    {
        var updater = new OneToOneNavigationPropertyUpdater<TRoot, T>(propertyUpdater, propertySelector);
        propertyUpdater.AddProperty(updater);
        return updater;
    }

    public static INavigationPropertyUpdater<T> ThenOne<TRoot, T, T1>(this INavigationPropertyUpdater<TRoot, T> navigationPropertyUpdater,
        Expression<Func<T, T1>> propertySelector)
        where TRoot : class
        where T : class
        where T1 : class
    {
        var rootUpdater = new RootUpdater<T>();
        var updater = new OneToOneNavigationPropertyUpdater<T, T1>(rootUpdater, propertySelector);

        rootUpdater.AddProperty(updater);
        navigationPropertyUpdater.AddRoot(rootUpdater);
        return rootUpdater;
    }

    public static INavigationPropertyUpdater<TRoot, T> UpdateList<TRoot, T, I>(this INavigationPropertyUpdater<TRoot> propertyUpdater,
        Expression<Func<TRoot, ICollection<T>>> propertySelector)
        where TRoot : class
        where T : class, IEntity<I>
        where I : struct
    {
        var updater = new OneToManyNavigationPropertyUpdate<TRoot, T, I>(propertyUpdater, propertySelector);
        propertyUpdater.AddProperty(updater);
        return updater;
    }

    public static INavigationPropertyUpdater<T> ThenList<TRoot, T, T1, I>(this INavigationPropertyUpdater<TRoot, T> navigationPropertyUpdater,
        Expression<Func<T, ICollection<T1>>> propertySelector)
        where TRoot : class
        where T : class
        where T1 : class, IEntity<I>
        where I : struct
    {
        var rootUpdater = new RootUpdater<T>();
        var updater = new OneToManyNavigationPropertyUpdate<T, T1, I>(rootUpdater, propertySelector);

        rootUpdater.AddProperty(updater);
        navigationPropertyUpdater.AddRoot(rootUpdater);
        return rootUpdater;
    }

    public static INavigationPropertyUpdater<TRoot, T> UpdateJunction<TRoot, T, I>(this INavigationPropertyUpdater<TRoot> propertyUpdater,
        Expression<Func<TRoot, ICollection<T>>> propertySelector)
        where TRoot : class
        where T : class, IEntity<I>
        where I : struct
    {
        var updater = new ManyToManyNavigationPropertyUpdate<TRoot, T, I>(propertyUpdater, propertySelector);
        propertyUpdater.AddProperty(updater);
        return updater;
    }

    public static INavigationPropertyUpdater<T> ThenJunction<TRoot, T, T1, I>(this INavigationPropertyUpdater<TRoot, T> navigationPropertyUpdater,
        Expression<Func<T, ICollection<T1>>> propertySelector)
        where TRoot : class
        where T : class
        where T1 : class, IEntity<I>
        where I : struct
    {
        var rootUpdater = new RootUpdater<T>();
        var updater = new ManyToManyNavigationPropertyUpdate<T, T1, I>(rootUpdater, propertySelector);

        rootUpdater.AddProperty(updater);
        navigationPropertyUpdater.AddRoot(rootUpdater);
        return rootUpdater;
    }
}

public class TestRepository(DbContext dbContext) : EFRepositoryBase<Test, int>(dbContext)
{
    public void Edit1(Test model)
    {
        //IQueryable<Test> v = dbContext.Set<Test>().Include(c => c.Level1).ThenInclude(c => c.)
        //HasNavigation().UpdateOne(c => c.Level1).ThenOne(c => c.Level2).UpdateOne(c => c.Level2);
    }
}

public class Test : EntityBase<int>, IAggregateRoot
{
    public Level1 Level1 { get; set; }

}

public class Level1 : EntityBase<int>
{
    public IList<Level2> Level2 { get; set; }
}

public class Level2 : EntityBase<int>
{

}
