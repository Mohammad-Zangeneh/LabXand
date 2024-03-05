namespace LabXand.Data
{
    public abstract class NavigationPropertyUpdaterBase<TRoot, TConfig>(TConfig updateConfiguration) : INavigationPropertyUpdater<TRoot>
        where TRoot : class
        where TConfig : IUpdateConfiguration<TRoot>
    {
        protected readonly TConfig updateConfiguration = updateConfiguration;

        public abstract void Update(TRoot currentValue, TRoot originalValue);
    }
}
