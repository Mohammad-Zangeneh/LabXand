using Microsoft.EntityFrameworkCore;

namespace LabXand.Infrastructure.Data.EF.UpdateConfiguration
{
    public interface INavigationPropertyUpdater<TRoot>
        where TRoot : class
    {
        void Update(DbContext context, TRoot currentValue, TRoot originalValue);       
        INavigationPropertyUpdaterCustomizer<TRoot> PropertyUpdaterCustomizer { get; }
    }
}
