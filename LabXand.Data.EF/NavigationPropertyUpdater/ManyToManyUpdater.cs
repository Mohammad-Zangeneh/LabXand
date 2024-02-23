using LabXand.Core;
using Microsoft.EntityFrameworkCore;

namespace LabXand.Infrastructure.Data.EF.UpdateConfiguration
{
    internal class ManyToManyUpdater<TRoot, T, I> : NavigationPropertyUpdaterBase<TRoot>
        where TRoot : class
        where T : class, IEntity<I>
        where I : struct
    {
        public ManyToManyUpdater(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, UpdateManyToManyCollection<TRoot, T, I> updateConfig)
            : base(propertyUpdaterCustomizer)
        {
            configuration = updateConfig;
        }

        public ManyToManyUpdater(UpdateManyToManyCollection<TRoot, T, I> updateConfig)
            : this(new EmptyPropertyUpdaterCustomizer<TRoot>(), updateConfig)
        {
        }
        UpdateManyToManyCollection<TRoot, T, I> configuration;
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
                // رکورد فعلی در دیتابیس موجود است و باید آپدیت شود
                if (currentValue != null)
                {
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
                // رکوردی که در دیتابیس وجود دارد در نسخه فعلی نیست و باید حذف شود
                else
                {
                    originalNavigationPropertyValue.Remove(originalValue);
                }
            }
            // مشخص کردن مواردی که جدید هستند و باید اضافه شوند
            ICollection<T> current = GetListValue(rootCurrentValue);

            var newValues = current.Except(originalNavigationPropertyValue);
            var newValueIds = newValues.Select(r => r.Id);
            var dbValues = context.Set<T>().Where(entity => newValueIds.Contains(entity.Id)).ToList();
            dbValues.ForEach(t => PropertyUpdaterCustomizer.OnBeforAddEntity(context, rootCurrentValue, t).Invoke(context, rootCurrentValue, t));
            originalNavigationPropertyValue.AddRange(dbValues);
            dbValues.ForEach(t => PropertyUpdaterCustomizer.OnAfterAddEntity(context, rootCurrentValue, t).Invoke(context, rootCurrentValue, t));
        }
    }
}
