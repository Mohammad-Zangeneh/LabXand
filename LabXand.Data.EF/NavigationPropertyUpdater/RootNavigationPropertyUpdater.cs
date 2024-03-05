using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF
{
    internal class RootNavigationPropertyUpdater<TRoot>(DbContext dbContext, IUpdateConfiguration<TRoot> configuration) : EFNavigationPropertyUpdaterBase<TRoot>(dbContext, configuration)

        where TRoot : class
    {
        public override void Update(TRoot currentValue, TRoot originalValue)
        {
            dbContext.SafeUpdate(currentValue, originalValue, updateConfiguration.ConstantFields);
            if (updateConfiguration.InnerConfigurations == null)
                return;
            foreach (var updateConfig in updateConfiguration.InnerConfigurations)
            {
                if (updateConfig is IUpdateConfiguration<TRoot> tempconfig)
                    tempconfig.CreateUpdater().Update(dbContext, currentValue, originalValue);
            }
        }
    }
}
