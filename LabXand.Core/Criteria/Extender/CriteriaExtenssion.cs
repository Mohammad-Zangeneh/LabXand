namespace LabXand.Core;

public static class CriteriaExtenssion
{
    public static Criteria And(this Criteria criteria, Criteria newCriteria)
    {
        return CriteriaBuilder.CreateAndCriteria(criteria.ObjectType, criteria, newCriteria);
    }

    public static Criteria Or(this Criteria criteria, Criteria newCriteria)
    {
        return CriteriaBuilder.CreateOrCriteria(criteria.ObjectType, criteria, newCriteria);
    }

    public static Criteria Equal(this Criteria criteria, string propertyName, object propertyValue, bool andBinaryOperation)
    {
        if (andBinaryOperation)
            return criteria.And(CriteriaBuilder.CreateEqualCriteria(criteria.ObjectType, propertyName, propertyValue));
        else
            return criteria.Or(CriteriaBuilder.CreateEqualCriteria(criteria.ObjectType, propertyName, propertyValue));
    }

    public static Criteria NotEqual(this Criteria criteria, string propertyName, object propertyValue, bool andBinaryOperation)
    {
        if (andBinaryOperation)
            return criteria.And(CriteriaBuilder.CreateNotEqualCriteria(criteria.ObjectType, propertyName, propertyValue));
        else
            return criteria.Or(CriteriaBuilder.CreateNotEqualCriteria(criteria.ObjectType, propertyName, propertyValue));
    }

    public static Criteria Like(this Criteria criteria, string propertyName, string propertyValue, bool andBinaryOperation)
    {
        if (andBinaryOperation)
            return criteria.And(CriteriaBuilder.CreateLikeCriteria(criteria.ObjectType, propertyName, propertyValue));
        else
            return criteria.Or(CriteriaBuilder.CreateLikeCriteria(criteria.ObjectType, propertyName, propertyValue));
    }

    public static Criteria GreaterThan(this Criteria criteria, string propertyName, object propertyValue, bool andBinaryOperation)
    {
        if (andBinaryOperation)
            return criteria.And(CriteriaBuilder.CreateGreaterThanCriteria(criteria.ObjectType, propertyName, propertyValue));
        else
            return criteria.Or(CriteriaBuilder.CreateGreaterThanCriteria(criteria.ObjectType, propertyName, propertyValue));
    }

    public static Criteria GreaterThanOrEqual(this Criteria criteria, string propertyName, object propertyValue, bool andBinaryOperation)
    {
        if (andBinaryOperation)
            return criteria.And(CriteriaBuilder.CreateGreaterThanOrEqualCriteria(criteria.ObjectType, propertyName, propertyValue));
        else
            return criteria.Or(CriteriaBuilder.CreateGreaterThanOrEqualCriteria(criteria.ObjectType, propertyName, propertyValue));
    }

    public static Criteria LessThan(this Criteria criteria, string propertyName, object propertyValue, bool andBinaryOperation)
    {
        if (andBinaryOperation)
            return criteria.And(CriteriaBuilder.CreateLessThanCriteria(criteria.ObjectType, propertyName, propertyValue));
        else
            return criteria.Or(CriteriaBuilder.CreateLessThanCriteria(criteria.ObjectType, propertyName, propertyValue));
    }

    public static Criteria LessThanOrEqual(this Criteria criteria, string propertyName, object propertyValue, bool andBinaryOperation)
    {
        if (andBinaryOperation)
            return criteria.And(CriteriaBuilder.CreateLessThanOrEqualCriteria(criteria.ObjectType, propertyName, propertyValue));
        else
            return criteria.Or(CriteriaBuilder.CreateLessThanOrEqualCriteria(criteria.ObjectType, propertyName, propertyValue));
    }

    public static Criteria CreateFromFilterModel<T>(T filterModel)
        where T : class
    {
        Criteria criteria = CriteriaBuilder.CreateNew<T>();
        Type modelType = typeof(T);
        foreach (var item in modelType.GetProperties())
        {
            object propertyValue = TypeHelper.GetPropertyValue(filterModel, item.Name);
            if (propertyValue != null)
            {
                if (item.PropertyType == typeof(string))
                {
                    if (!string.IsNullOrWhiteSpace(propertyValue.ToString()))
                        criteria = criteria.Like(item.Name, propertyValue.ToString(), true);
                }
                else if (item.PropertyType == typeof(bool))
                {
                    criteria = criteria.Equal(item.Name, propertyValue, true);
                }
                else if (!propertyValue.Equals(TypeHelper.GetDefaultValue(item.PropertyType)))
                    criteria = criteria.Equal(item.Name, propertyValue, true);
            }
        }
        return criteria;
    }
}
