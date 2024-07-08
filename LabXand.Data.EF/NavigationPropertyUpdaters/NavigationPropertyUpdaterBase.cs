namespace LabXand.Data.EF;

public abstract class NavigationPropertyUpdaterBase<TRoot, T>(INavigationPropertyUpdater<TRoot> root) :
    RootPropertyUpdaterBase<TRoot>, INavigationPropertyUpdater<TRoot, T>
    where T : class
    where TRoot : class
{
    private readonly INavigationPropertyUpdater<TRoot> root = root;

    public INavigationPropertyUpdater<TRoot> Root => root;
    public override INavigationPropertyUpdater<TRoot, T> AddProperty<T>(INavigationPropertyUpdater<TRoot, T> navigationPropertyUpdater)
    {
        return root.AddProperty(navigationPropertyUpdater);
    }
}