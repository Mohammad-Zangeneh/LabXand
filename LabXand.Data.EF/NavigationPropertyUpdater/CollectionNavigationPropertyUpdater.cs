using LabXand.Core;
using Microsoft.EntityFrameworkCore;

namespace LabXand.Infrastructure.Data.EF.UpdateConfiguration
{
    internal class CollectionNavigationPropertyUpdater<TRoot, T, I> : NavigationPropertyUpdaterBase<TRoot>
        where TRoot : class
        where T : class, IEntity<I>
        where I : struct
    {
        public CollectionNavigationPropertyUpdater(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, UpdateCollectionConfiguration<TRoot, T, I> updateConfig)
            : base(propertyUpdaterCustomizer)
        {
            configuration = updateConfig;
        }
        public CollectionNavigationPropertyUpdater(UpdateCollectionConfiguration<TRoot, T, I> updateConfig)
            : this(new EmptyPropertyUpdaterCustomizer<TRoot>(), updateConfig)
        {
        }
        UpdateCollectionConfiguration<TRoot, T, I> configuration;
        protected ICollection<T> GetListValue(TRoot parent)
        {
            if (parent != null)
            {
                if (configuration.ItemSelector != null)
                {
                    var temp = configuration.ItemSelector.Compile().Invoke(parent);
                    if (temp != null)
                        return temp;
                    else
                        return new List<T>();
                }
                else
                    return new List<T>();
            }
            else
                return new List<T>();
        }

        public override void Update(DbContext context, TRoot rootCurrentValue, TRoot rootOriginalValue)
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
                    context.SafeUpdate<T>(currentValue, originalValue, configuration.ConstantFields);
                    if (configuration.InnerConfigurations != null)
                    {
                        foreach (var updateConfig in configuration.InnerConfigurations)
                        {
                            IUpdateConfiguration<T> tempconfig = updateConfig as IUpdateConfiguration<T>;
                            if (tempconfig != null)
                                tempconfig.CreateUpdater().Update(context, currentValue, originalValue);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(configuration.ParentPropertyName))
                        TypeHelper.SetPropertyValue(originalValue, configuration.ParentPropertyName, null);

                    if (!string.IsNullOrWhiteSpace(configuration.ChildtPropertyName))
                        TypeHelper.SetPropertyValue(originalValue, configuration.ChildtPropertyName, null);

                    PropertyUpdaterCustomizer.OnBeforRemoveEntity(context, rootOriginalValue, originalValue).Invoke(context, rootOriginalValue, originalValue); ;
                    context.Entry(originalValue).State = EntityState.Deleted;
                    PropertyUpdaterCustomizer.OnAfterRemoveEntity(context, rootOriginalValue, originalValue).Invoke(context, rootOriginalValue, originalValue); ;
                }
            }
            // مشخص کردن مواردی که جدید هستند و باید اضافه شوند
            IEnumerable<T> current = GetListValue(rootCurrentValue);
            ICollection<T> values = GetListValue(rootOriginalValue);
            T[] tempArray = new T[values.Count];
            values.CopyTo(tempArray, 0);
            foreach (var item in current)
            {
                T temp = tempArray.FirstOrDefault(o => o.Id.Equals(item.Id));
                if (temp == null)
                {
                    PropertyUpdaterCustomizer.OnBeforAddEntity(context, rootCurrentValue, item).Invoke(context, rootCurrentValue, item);
                    context.Entry(item).State = EntityState.Added;
                    PropertyUpdaterCustomizer.OnAfterAddEntity(context, rootCurrentValue, item).Invoke(context, rootCurrentValue, item);
                }
            }
        }
    }
}
