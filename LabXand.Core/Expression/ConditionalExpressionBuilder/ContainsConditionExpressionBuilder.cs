using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace LabXand.Core
{
    public class ContainsConditionExpressionBuilder : IConditionalExpressionBuilder
    {
        Expression IConditionalExpressionBuilder.Get(MemberExpression memberExpression, object value, Type valueType)
        {

            List<object> objectList = ((IEnumerable)value).Cast<object>().ToList();
            if (objectList.Count == 0)
                return Expression.Constant(true);

            ConstantExpression keyExpression = Expression.Constant(value, valueType);

            var listType = typeof(List<>);
            Type expressionType = memberExpression.Type;
            var constructedListType = listType.MakeGenericType(expressionType);
            var instance = Activator.CreateInstance(constructedListType);
            foreach (var item in objectList)
            {
                ((IList)instance).Add(Convert.ChangeType(item, memberExpression.Type.IsNullable() ? memberExpression.Type.GetGenericArguments()[0] : memberExpression.Type));
            }
            MethodInfo containsMethod = typeof(Enumerable).
                    GetMethods().
                    Where(x => x.Name == "Contains").
                    Single(x => x.GetParameters().Length == 2).
                    MakeGenericMethod(expressionType);

            if (containsMethod != null)
            {
                return Expression.Call(containsMethod, Expression.Constant(instance, instance.GetType()), memberExpression);
            }
            throw new Exception("");
        }
    }
}
