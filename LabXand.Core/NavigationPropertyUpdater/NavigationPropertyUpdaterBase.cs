using Microsoft.EntityFrameworkCore;

namespace LabXand.Data
{
    public abstract class NavigationPropertyUpdaterBase<TRoot> : INavigationPropertyUpdater<TRoot>
        where TRoot : class
    {
        protected NavigationPropertyUpdaterBase(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
        {
            _propertyUpdaterCustomizer = propertyUpdaterCustomizer;
        }
        public abstract void Update(DbContext context, TRoot currentValue, TRoot originalValue);
        INavigationPropertyUpdaterCustomizer<TRoot> _propertyUpdaterCustomizer;
        public INavigationPropertyUpdaterCustomizer<TRoot> PropertyUpdaterCustomizer
        {
            get
            { return _propertyUpdaterCustomizer; }
        }
    }
}
