using LabXand.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF
{
    //public static class RepositoryExtender
    //{
    //    public static void Edit<T>(this IRepository<T> repository) where T : IAgregateRoot 
    //    {
    //    }
    //}
    //public class UpdateConfiguration
    //{

    //}
    internal class ManyToManyNavigationPropertyUpdater<TRoot, T, I>(DbContext dbContext, ManyToManyUpdateConfiguration<TRoot, T, I> updateConfig) :
        EFNavigationPropertyUpdaterBase<TRoot, ManyToManyUpdateConfiguration<TRoot, T, I>>(dbContext, updateConfig)
        where TRoot : class
        where T : class, IEntity<I>
        where I : struct
    {
        protected ICollection<T> GetListValue(TRoot parent)
        {
            if (parent != null)
            {
                if (updateConfiguration.ItemSelector != null)
                {
                    var temp = updateConfiguration.ItemSelector.Compile().Invoke(parent);
                    if (temp != null)
                        return temp;
                    else
                        return [];
                }
                else
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
                // رکورد فعلی در دیتابیس موجود است و باید آپدیت شود
                if (currentValue != null)
                {
                    if (updateConfiguration.InnerConfigurations != null)
                    {
                        foreach (var updateConfig in updateConfiguration.InnerConfigurations)
                        {
                            IUpdateConfiguration<T> tempconfig = updateConfig as IUpdateConfiguration<T>;
                            //if (tempconfig != null)
                            //    tempconfig.CreateUpdater().Update(dbContext, currentValue, originalValue);
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
            var current = GetListValue(rootCurrentValue);

            var newValues = current.Except(originalNavigationPropertyValue);
            var newValueIds = newValues.Select(r => r.Id);
            var dbValues = dbContext.Set<T>().Where(entity => newValueIds.Contains(entity.Id)).ToList();
            //dbValues.ForEach(t => PropertyUpdaterCustomizer.OnBeforAddEntity(context, rootCurrentValue, t).Invoke(context, rootCurrentValue, t));
            ((List<T>)originalNavigationPropertyValue).AddRange(dbValues);
            //dbValues.ForEach(t => PropertyUpdaterCustomizer.OnAfterAddEntity(context, rootCurrentValue, t).Invoke(context, rootCurrentValue, t));
        }
    }
}
