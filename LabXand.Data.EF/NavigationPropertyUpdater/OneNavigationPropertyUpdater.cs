using LabXand.Core;
using Microsoft.EntityFrameworkCore;

namespace LabXand.Infrastructure.Data.EF.UpdateConfiguration
{
    internal class OneNavigationPropertyUpdater<TRoot, T, I> : NavigationPropertyUpdaterBase<TRoot>
        where TRoot : class
        where T : class, IEntity<I>
        where I : struct
    {
        public OneNavigationPropertyUpdater(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, UpdateOneEntityConfiguration<TRoot, T, I> updateConfig)
            : base(propertyUpdaterCustomizer)
        {
            configuration = updateConfig;
        }
        public OneNavigationPropertyUpdater(UpdateOneEntityConfiguration<TRoot, T, I> updateConfig)
            : this(new EmptyPropertyUpdaterCustomizer<TRoot>(), updateConfig)
        {
        }
        UpdateOneEntityConfiguration<TRoot, T, I> configuration;

        protected T GetValue(TRoot parent)
        {
            if (parent != null)
            {
                if (configuration.ItemSelector != null)
                {
                    var temp = configuration.ItemSelector.Compile().Invoke(parent);
                    if (temp != null)
                        return temp;
                    else
                        return null;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public override void Update(DbContext context, TRoot currentValue, TRoot originalValue)
        {
            T current = GetValue(currentValue);
            T original = GetValue(originalValue);
            if (current == null && original == null)
                return;

            if (original != null)
            {
                if (current != null)
                {
                    context.SafeUpdate<T>(current, original, configuration.ConstantFields);
                    if (configuration.InnerConfigurations != null)
                    {
                        foreach (var updateConfig in configuration.InnerConfigurations)
                        {
                            IUpdateConfiguration<T> tempconfig = updateConfig as IUpdateConfiguration<T>;
                            if (tempconfig != null)
                                tempconfig.CreateUpdater().Update(context, current, original);
                        }
                    } 
                }
                else
                {
                    PropertyUpdaterCustomizer.OnBeforRemoveEntity(context, currentValue, original).Invoke(context, currentValue, original);
                    context.Entry(original).State = EntityState.Deleted;
                    PropertyUpdaterCustomizer.OnAfterRemoveEntity(context, currentValue, original).Invoke(context, currentValue, original);
                }
            }
            else
            {
                PropertyUpdaterCustomizer.OnBeforAddEntity(context, currentValue, current).Invoke(context, currentValue, current);
                context.Entry(current).State = EntityState.Added;
                PropertyUpdaterCustomizer.OnAfterAddEntity(context, currentValue, current).Invoke(context, currentValue, current);
            }
        }
    }
}
