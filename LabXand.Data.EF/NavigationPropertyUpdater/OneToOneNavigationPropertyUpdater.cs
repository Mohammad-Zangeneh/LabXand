using LabXand.Data;
using LabXand.Data.EF;
using LabXand.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace LabXand.Infrastructure.Data.EF.UpdateConfiguration
{
    internal class OneToOneNavigationPropertyUpdater<TRoot, T, I>(DbContext dbContext, OneToOneUpdateConfiguration<TRoot, T, I> updateConfig) : EFNavigationPropertyUpdaterBase<TRoot, OneToOneUpdateConfiguration<TRoot, T, I>>(dbContext, updateConfig)
        where TRoot : class
        where T : class, IEntity<I>
        where I : struct
    {
        protected T GetValue(TRoot parent)
        {
            if (parent != null)
            {
                if (updateConfiguration.ItemSelector != null)
                {
                    var temp = updateConfiguration.ItemSelector.Compile().Invoke(parent);
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

        public override void Update(TRoot currentValue, TRoot originalValue)
        {
            T current = GetValue(currentValue);
            T original = GetValue(originalValue);
            if (current == null && original == null)
                return;

            if (original != null)
            {
                if (current != null)
                {
                    dbContext.SafeUpdate<T>(current, original, updateConfiguration.ConstantFields);
                    if (updateConfiguration.InnerConfigurations != null)
                    {
                        foreach (var updateConfig in updateConfiguration.InnerConfigurations)
                        {
                            if (updateConfig is IUpdateConfiguration<T> tempconfig)
                                tempconfig.CreateUpdater().Update(dbContext, current, original);
                        }
                    }
                }
                else
                {
                    //PropertyUpdaterCustomizer.OnBeforRemoveEntity(context, currentValue, original).Invoke(context, currentValue, original);
                    dbContext.Entry(original).State = EntityState.Deleted;
                    //PropertyUpdaterCustomizer.OnAfterRemoveEntity(context, currentValue, original).Invoke(context, currentValue, original);
                }
            }
            else
            {
                //PropertyUpdaterCustomizer.OnBeforAddEntity(context, currentValue, current).Invoke(context, currentValue, current);
                dbContext.Entry(current).State = EntityState.Added;
                //PropertyUpdaterCustomizer.OnAfterAddEntity(context, currentValue, current).Invoke(context, currentValue, current);
            }
        }
    }
}
