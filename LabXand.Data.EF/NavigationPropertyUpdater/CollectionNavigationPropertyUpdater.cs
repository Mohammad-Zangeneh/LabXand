using LabXand.Core;
using LabXand.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF
{
    internal class CollectionNavigationPropertyUpdater<TRoot, T, I>(DbContext dbContext, INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, UpdateCollectionConfiguration<TRoot, T, I> updateConfig) : NavigationPropertyUpdaterBase<TRoot>(propertyUpdaterCustomizer)
        where TRoot : class
        where T : class, IEntity<I>
        where I : struct
    {
        public CollectionNavigationPropertyUpdater(UpdateCollectionConfiguration<TRoot, T, I> updateConfig)
            : this(new EmptyPropertyUpdaterCustomizer<TRoot>(), updateConfig)
        {
        }

        protected ICollection<T> GetListValue(TRoot parent)
        {
            if (updateConfig.ItemSelector != null)
            {
                var temp = updateConfig.ItemSelector.Compile().Invoke(parent);
                if (temp != null)
                    return temp;
                return [];
            }
            else
                return [];
        }

        public override void Update(TRoot rootCurrentValue, TRoot rootOriginalValue)
        {
            ICollection<T> originalNavigationPropertyValue = GetListValue(rootOriginalValue);
            T[] itterationArray = new T[originalNavigationPropertyValue.Count];
            originalNavigationPropertyValue.CopyTo(itterationArray, 0);
            // مشخص کردن مواردی که باید ویرایش و یا حذف شوند
            foreach (T originalValue in itterationArray)
            {
                T currentValue = GetListValue(rootCurrentValue).FirstOrDefault(o => o.Id.Equals(originalValue.Id));
                if (currentValue != null)
                {
                    //context.Entry(temp).State = EntityState.Modified;
                    dbContext.SafeUpdate<T>(currentValue, originalValue, updateConfig.ConstantFields);
                    if (updateConfig.InnerConfigurations != null)
                    {
                        foreach (var updateConfig in updateConfig.InnerConfigurations)
                        {
                            IUpdateConfiguration<T> tempconfig = updateConfig as IUpdateConfiguration<T>;
                            if (tempconfig != null)
                                tempconfig.CreateUpdater().Update(dbContext, currentValue, originalValue);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(updateConfig.ParentPropertyName))
                        TypeHelper.SetPropertyValue(originalValue, updateConfig.ParentPropertyName, null);

                    if (!string.IsNullOrWhiteSpace(updateConfig.ChildtPropertyName))
                        TypeHelper.SetPropertyValue(originalValue, updateConfig.ChildtPropertyName, null);

                    PropertyUpdaterCustomizer.OnBeforRemoveEntity(context, rootOriginalValue, originalValue).Invoke(context, rootOriginalValue, originalValue); ;
                    dbContext.Entry(originalValue).State = EntityState.Deleted;
                    PropertyUpdaterCustomizer.OnAfterRemoveEntity(context, rootOriginalValue, originalValue).Invoke(context, rootOriginalValue, originalValue); ;
                }
            }
            // مشخص کردن مواردی که جدید هستند و باید اضافه شوند
            var current = GetListValue(rootCurrentValue);
            var values = GetListValue(rootOriginalValue);
            var tempArray = new T[values.Count];
            values.CopyTo(tempArray, 0);
            foreach (var item in current)
            {
                var temp = tempArray.FirstOrDefault(o => o.Id.Equals(item.Id));
                if (temp != null) continue;
                PropertyUpdaterCustomizer.OnBeforAddEntity(rootCurrentValue, item).Invoke(rootCurrentValue, item);
                dbContext.Entry(item).State = EntityState.Added;
                PropertyUpdaterCustomizer.OnAfterAddEntity(rootCurrentValue, item).Invoke(rootCurrentValue, item);
            }
        }
    }
}
