namespace LabXand.Data
{
    public interface INavigationPropertyUpdater<TRoot>
        where TRoot : class
    {
        void Update(TRoot currentValue, TRoot originalValue);
    }
}
