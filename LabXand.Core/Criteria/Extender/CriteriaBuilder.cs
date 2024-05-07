using System.Linq.Expressions;

namespace LabXand.Core;

public static class CriteriaBuilder
{
    #region Root
    public static Criteria CreateNew<T>()
        where T : class
    {
        return new EmptyCriteria() { ObjectType = typeof(T) };
    }
    #endregion

    #region Equal
    internal static Criteria CreateEqualCriteria(Type objectType, object leftExpression, object rightExpression)
    {
        return new EqualCriteria { FirstOprand = leftExpression, SecondOperand = rightExpression, ObjectType = objectType };
    }

    internal static Criteria CreateEqualCriteria<T>(object leftExpression, object rightExpression) where T : class
    {
        return CreateEqualCriteria(typeof(T), leftExpression, rightExpression);
    }
    #endregion

    #region NotEqual
    internal static Criteria CreateNotEqualCriteria(Type objectType, object leftExpression, object rightExpression)
    {
        return new NotEqualCriteria { FirstOprand = leftExpression, SecondOperand = rightExpression, ObjectType = objectType };
    }

    internal static Criteria CreateNotEqualCriteria<T>(object leftExpression, object rightExpression) where T : class
    {
        return CreateNotEqualCriteria(typeof(T), leftExpression, rightExpression);
    }
    #endregion

    #region GreaterThan
    internal static Criteria CreateGreaterThanCriteria(Type objectType, object leftExpression, object rightExpression)
    {
        return new GreaterThan { FirstOprand = leftExpression, SecondOperand = rightExpression, ObjectType = objectType };
    }

    internal static Criteria CreateGreaterThanCriteria<T>(object leftExpression, object rightExpression) where T : class
    {
        return CreateGreaterThanCriteria(typeof(T), leftExpression, rightExpression);
    }
    #endregion

    #region GreaterThanOrEqual
    internal static Criteria CreateGreaterThanOrEqualCriteria(Type objectType, object leftExpression, object rightExpression)
    {
        return new GreaterThanOrEqual { FirstOprand = leftExpression, SecondOperand = rightExpression, ObjectType = objectType };
    }

    internal static Criteria CreateGreaterThanOrEqualCriteria<T>(object leftExpression, object rightExpression) where T : class
    {
        return CreateGreaterThanOrEqualCriteria(typeof(T), leftExpression, rightExpression);
    }
    #endregion

    #region LessThan
    internal static Criteria CreateLessThanCriteria(Type objectType, object leftExpression, object rightExpression)
    {
        return new LessThan { FirstOprand = leftExpression, SecondOperand = rightExpression, ObjectType = objectType };
    }

    internal static Criteria CreateLessThanCriteria<T>(object leftExpression, object rightExpression) where T : class
    {
        return CreateLessThanCriteria(typeof(T), leftExpression, rightExpression);
    }
    #endregion

    #region LessThanOrEqual
    internal static Criteria CreateLessThanOrEqualCriteria(Type objectType, object leftExpression, object rightExpression)
    {
        return new LessThanOrEqual { FirstOprand = leftExpression, SecondOperand = rightExpression, ObjectType = objectType };
    }

    internal static Criteria CreateLessThanOrEqualCriteria<T>(object leftExpression, object rightExpression) where T : class
    {
        return CreateLessThanOrEqualCriteria(typeof(T), leftExpression, rightExpression);
    }
    #endregion

    #region Like
    internal static Criteria CreateLikeCriteria(Type objectType, object leftExpression, object rightExpression)
    {
        return new LikeCriteria { FirstOprand = leftExpression, SecondOperand = rightExpression, ObjectType = objectType };
    }

    internal static Criteria CreateLikeCriteria<T>(object leftExpression, object rightExpression) where T : class
    {
        return CreateLikeCriteria(typeof(T), leftExpression, rightExpression);
    }
    #endregion

    #region And
    internal static Criteria CreateAndCriteria(Type objectType, Criteria leftCriteria, Criteria rightCriteria)
    {
        return new AndCriteria { ObjectType = objectType, FirstOprand = leftCriteria, SecondOperand = rightCriteria };
    }

    internal static Criteria CreateAndCriteria<T>(Criteria leftCriteria, Criteria rightCriteria) where T : class
    {
        return CreateAndCriteria(typeof(T), leftCriteria, rightCriteria);
    }

    internal static Criteria CreateAndCriteria(Type objectType, object leftCriteria, object rightCriteria)
    {
        return new AndCriteria { ObjectType = objectType, FirstOprand = leftCriteria, SecondOperand = rightCriteria };
    }

    internal static Criteria CreateAndCriteria<T>(object leftCriteria, object rightCriteria) where T : class
    {
        return CreateAndCriteria(typeof(T), leftCriteria, rightCriteria);
    }
    #endregion

    #region Or
    internal static Criteria CreateOrCriteria(Type objectType, Criteria leftCriteria, Criteria rightCriteria)
    {
        return new OrCriteria { ObjectType = objectType, FirstOprand = leftCriteria, SecondOperand = rightCriteria };
    }

    internal static Criteria CreateOrCriteria<T>(Criteria leftCriteria, Criteria rightCriteria) where T : class
    {
        return CreateOrCriteria(typeof(T), leftCriteria, rightCriteria);
    }


    internal static Criteria CreateOrCriteria(Type objectType, object leftCriteria, object rightCriteria)
    {
        return new OrCriteria { ObjectType = objectType, FirstOprand = leftCriteria, SecondOperand = rightCriteria };
    }

    internal static Criteria CreateOrCriteria<T>(object leftCriteria, object rightCriteria) where T : class
    {
        return CreateOrCriteria(typeof(T), leftCriteria, rightCriteria);
    }
    #endregion

    #region IsNull
    internal static Criteria CreateIsNullCriteria(Type objectType, object leftExpression)
    {
        return new IsNullCriteria { FirstOprand = leftExpression, ObjectType = objectType };
    }

    internal static Criteria CreateIsNullCriteria<T>(object leftExpression) where T : class
    {
        return CreateIsNullCriteria(typeof(T), leftExpression);
    }
    #endregion

    #region NotEqual
    internal static Criteria CreateIsNotNullCriteria(Type objectType, object leftExpression)
    {
        return new IsNotNullCriteria { FirstOprand = leftExpression, ObjectType = objectType };
    }

    internal static Criteria CreateIsNotNullCriteria<T>(object leftExpression) where T : class
    {
        return CreateIsNotNullCriteria(typeof(T), leftExpression);
    }
    #endregion

    #region Contains
    internal static Criteria CreateContainsCriteria(Type objectType, object leftExpression, object rightExpression)
    {
        return new ContainsCriteria { FirstOprand = leftExpression, SecondOperand = rightExpression, ObjectType = objectType };
    }

    internal static Criteria CreateContainsCriteria<T>(object leftExpression, object rightExpression) where T : class
    {
        return CreateContainsCriteria(typeof(T), leftExpression, rightExpression);
    }
    #endregion

    public static Criteria CreateFromFilterOperation<T>(FilterOperations filterOperation, string propertyName, object propertyValue)
        where T : class
    {
        if (propertyValue != null && !string.IsNullOrWhiteSpace(propertyName))
        {
            Criteria criteria = new EmptyCriteria();
            switch (filterOperation)
            {
                case FilterOperations.Equal:
                    criteria = CreateEqualCriteria<T>(propertyName, propertyValue);
                    break;
                case FilterOperations.Like:
                    criteria = CreateLikeCriteria<T>(propertyName, propertyValue);
                    break;
                case FilterOperations.NotEqual:
                    criteria = CreateNotEqualCriteria<T>(propertyName, propertyValue);
                    break;
                case FilterOperations.GreaterThan:
                    criteria = CreateGreaterThanCriteria<T>(propertyName, propertyValue);
                    break;
                case FilterOperations.GreaterThanOrEqual:
                    criteria = CreateGreaterThanOrEqualCriteria<T>(propertyName, propertyValue);
                    break;
                case FilterOperations.LessThan:
                    criteria = CreateLessThanCriteria<T>(propertyName, propertyValue);
                    break;
                case FilterOperations.LessThanOrEqual:
                    criteria = CreateLessThanOrEqualCriteria<T>(propertyName, propertyValue);
                    break;
                case FilterOperations.IsNull:
                    criteria = CreateIsNullCriteria<T>(propertyName);
                    break;
                case FilterOperations.IsNotNull:
                    criteria = CreateIsNotNullCriteria<T>(propertyName);
                    break;
                case FilterOperations.Between:
                    string s = propertyValue.ToString();
                    string[] values = s.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length == 2)
                    {
                        object fromValue = values[0];
                        object toValue = values[1];
                        criteria = CreateGreaterThanOrEqualCriteria<T>(propertyName, fromValue).And(CreateLessThanOrEqualCriteria<T>(propertyName, toValue));
                    }
                    break;
                case FilterOperations.Contains:
                    criteria = CreateContainsCriteria<T>(propertyName, propertyValue);
                    break;
                default:
                    break;
            }
            return criteria;
        }
        if (propertyValue == null)
        {
            if (filterOperation == FilterOperations.IsNull)
                return CreateIsNullCriteria<T>(propertyName);
            if (filterOperation == FilterOperations.IsNotNull)
                return CreateIsNotNullCriteria<T>(propertyName);
        }
        return new EmptyCriteria();
    }

    //internal static Criteria<T> CreateHierarchicalCriteria<T>(object leftCriteria, object rightCriteria) where T : class
    //{
    //    return new HierarchicalCriteria<T> { FirstOprand = leftCriteria, SecondOperand = rightCriteria };
    //}

    internal static Expression<Func<T, bool>> CreateLambdaExpression<T>(Criteria criteria, ParameterExpression parameter) where T : class
    {
        if (criteria == null)
            throw new Exception();

        if (parameter == null)
            parameter = Expression.Parameter(typeof(T));

        Expression lambda = criteria.GetExpression(parameter);

        if (lambda == null)
            throw new Exception();

        return Expression.Lambda<Func<T, bool>>(lambda, parameter);
    }
}
