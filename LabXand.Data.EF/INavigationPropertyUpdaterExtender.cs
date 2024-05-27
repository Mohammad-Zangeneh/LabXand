using LabXand.SharedKernel;
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
        var rootUpdater = new RootUpdater<T>(GetMemberName(propertySelector));
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
        var updater = new OneToManyNavigationPropertyUpdater<TRoot, T, I>(propertyUpdater, propertySelector);
        propertyUpdater.AddProperty(updater);
        return updater;
    }

    public static INavigationPropertyUpdater<TRoot, T> UpdateImutableList<TRoot, T, I>(this INavigationPropertyUpdater<TRoot> propertyUpdater,
        Expression<Func<TRoot, ICollection<T>>> propertySelector)
        where TRoot : class
        where T : class, IEntity<I>
        where I : struct
    {
        var updater = new OneToManyImutableNavigationPropertyUpdater<TRoot, T, I>(propertyUpdater, propertySelector);
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
        var rootUpdater = new RootUpdater<T>(GetMemberName(propertySelector));
        var updater = new OneToManyNavigationPropertyUpdater<T, T1, I>(rootUpdater, propertySelector);

        rootUpdater.AddProperty(updater);
        navigationPropertyUpdater.AddRoot(rootUpdater);
        return rootUpdater;
    }

    public static INavigationPropertyUpdater<T> ThenImutableList<TRoot, T, T1, I>(this INavigationPropertyUpdater<TRoot, T> navigationPropertyUpdater,
        Expression<Func<T, ICollection<T1>>> propertySelector)
        where TRoot : class
        where T : class
        where T1 : class, IEntity<I>
        where I : struct
    {
        var rootUpdater = new RootUpdater<T>(GetMemberName(propertySelector));
        var updater = new OneToManyImutableNavigationPropertyUpdater<T, T1, I>(rootUpdater, propertySelector);

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
        var updater = new ManyToManyNavigationPropertyUpdater<TRoot, T, I>(propertyUpdater, propertySelector);
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
        var rootUpdater = new RootUpdater<T>(GetMemberName(propertySelector));
        var updater = new ManyToManyNavigationPropertyUpdater<T, T1, I>(rootUpdater, propertySelector);

        rootUpdater.AddProperty(updater);
        navigationPropertyUpdater.AddRoot(rootUpdater);
        return rootUpdater;
    }

    static string GetMemberName<TRoot, T>(Expression<Func<TRoot, T>> propertyAccessor)
        where TRoot : class
        where T : class
        => ((MemberExpression)propertyAccessor.Body).Member.Name;
}