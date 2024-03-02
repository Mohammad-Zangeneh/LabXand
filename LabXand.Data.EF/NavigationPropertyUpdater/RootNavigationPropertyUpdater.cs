using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF
{
    internal class RootNavigationPropertyUpdater<TRoot>(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, IUpdateConfiguration<TRoot> configuration) : NavigationPropertyUpdaterBase<TRoot>(propertyUpdaterCustomizer)
        where TRoot : class
    {
        public RootNavigationPropertyUpdater(IUpdateConfiguration<TRoot> configuration)
            : this(new EmptyPropertyUpdaterCustomizer<TRoot>(), configuration)
        {

        }

        readonly IUpdateConfiguration<TRoot> updateConfiguration = configuration;
        public override void Update(DbContext context, TRoot currentValue, TRoot originalValue)
        {
            context.SafeUpdate(currentValue, originalValue, updateConfiguration.ConstantFields);
            if (updateConfiguration.InnerConfigurations == null)
                return;
            foreach (var updateConfig in updateConfiguration.InnerConfigurations)
            {
                if (updateConfig is IUpdateConfiguration<TRoot> tempconfig)
                    tempconfig.CreateUpdater().Update(context, currentValue, originalValue);
            }
        }
    }
}
