using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF;

public interface INavigationPropertyUpdater
{
    string Key { get; }
    void Update(DbContext dbContext, object currentValue, object originalValue);
    List<string> GetIncludePath();
}

public interface INavigationPropertyUpdater<TRoot> : INavigationPropertyUpdater
    where TRoot : class
{
    INavigationPropertyUpdater<T> AddRoot<T>(INavigationPropertyUpdater<T> rootUpdater) where T : class;
    INavigationPropertyUpdater<TRoot, T> AddProperty<T>(INavigationPropertyUpdater<TRoot, T> navigationPropertyUpdater) where T : class;
    void Update(DbContext dbContext, TRoot current, TRoot original);
}

public interface INavigationPropertyUpdater<TRoot, T> : INavigationPropertyUpdater<TRoot>
    where TRoot : class
    where T : class
{
    INavigationPropertyUpdater<TRoot> Root { get; }
}