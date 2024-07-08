using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF;

public abstract class RootPropertyUpdaterBase<TRoot>() : INavigationPropertyUpdater<TRoot>
    where TRoot : class
{
    public RootPropertyUpdaterBase(INavigationPropertyUpdaterCustomizer<TRoot> navigationPropertyUpdaterCustomizer) : this()
    => this.navigationPropertyUpdaterCustomizer = navigationPropertyUpdaterCustomizer;
    protected readonly List<INavigationPropertyUpdater> navigationPropertyUpdaters = [];
    protected readonly INavigationPropertyUpdaterCustomizer<TRoot>? navigationPropertyUpdaterCustomizer;

    public abstract string Key { get; }

    public List<string> GetIncludePath()
    {
        List<string> path = [];
        if (!string.IsNullOrWhiteSpace(Key))
            path.Add(Key);
        path.AddRange(navigationPropertyUpdaters.SelectMany(n => n.GetIncludePath().Select(i => string.IsNullOrWhiteSpace(Key) ? i : $"{Key}.{i}")));
        return path.Distinct().ToList();
    }
    public void Update(DbContext dbContext, object current, object original) => Update(dbContext, (TRoot)current, (TRoot)original);

    public abstract void Update(DbContext dbContext, TRoot current, TRoot original);

    public virtual INavigationPropertyUpdater<TRoot, T> AddProperty<T>(INavigationPropertyUpdater<TRoot, T> navigationPropertyUpdater)
        where T : class
    {
        navigationPropertyUpdaters.Add(navigationPropertyUpdater);
        return navigationPropertyUpdater;
    }

    public INavigationPropertyUpdater<T> AddRoot<T>(INavigationPropertyUpdater<T> rootUpdater)
        where T : class
    {
        navigationPropertyUpdaters.Add(rootUpdater);
        return rootUpdater;
    }
}
