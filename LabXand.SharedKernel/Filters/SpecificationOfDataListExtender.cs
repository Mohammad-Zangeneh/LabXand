using LabXand.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace LabXand.Core
{
    public static class SpecificationOfDataListExtender
    {
        public static object GetValue<T>(this SpecificationOfDataList<T> specificationOfDataList,
            Expression<Func<T, dynamic>> propertySelector)
            where T : class
        {
            if (specificationOfDataList != null)
            {
                if (specificationOfDataList.FilterSpecifications != null)
                {
                    string propertyName = ExpressionHelper.GetNameOfProperty(propertySelector);
                    if (specificationOfDataList.FilterSpecifications.Any(t => t.PropertyName.Equals(propertyName)))
                        return specificationOfDataList.FilterSpecifications.First(t => t.PropertyName.Equals(propertyName)).FilterValue;
                }
            }
            return null;
        }
    }
}
