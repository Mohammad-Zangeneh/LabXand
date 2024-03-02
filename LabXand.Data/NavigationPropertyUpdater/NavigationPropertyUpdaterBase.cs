namespace LabXand.Data
{
    public abstract class NavigationPropertyUpdaterBase<TRoot>(IUpdateConfiguration<TRoot> updateConfiguration) : INavigationPropertyUpdater<TRoot>
        where TRoot : class
    {
        protected readonly IUpdateConfiguration<TRoot> updateConfiguration = updateConfiguration;

        public abstract void Update(TRoot currentValue, TRoot originalValue);
    }
}
