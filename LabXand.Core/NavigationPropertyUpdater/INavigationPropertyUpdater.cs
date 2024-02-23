using Microsoft.EntityFrameworkCore;

namespace LabXand.Data
{
    public interface INavigationPropertyUpdater<TRoot>
        where TRoot : class
    {
        void Update(DbContext context, TRoot currentValue, TRoot originalValue);       
        INavigationPropertyUpdaterCustomizer<TRoot> PropertyUpdaterCustomizer { get; }
    }
}
