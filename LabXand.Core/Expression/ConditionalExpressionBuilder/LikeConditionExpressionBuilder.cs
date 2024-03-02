using System.Linq.Expressions;
using System.Reflection;

namespace LabXand.Core
{
    public class LikeConditionExpressionBuilder : IConditionalExpressionBuilder
    {
        Expression IConditionalExpressionBuilder.Get(MemberExpression memberExpression, object value, Type valueType)
        {
            ConstantExpression keyExpression = Expression.Constant(value, valueType);
            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            if (containsMethod != null)
            {
                return Expression.Call(memberExpression, containsMethod, keyExpression);
            }
            throw new Exception("");
        }
    }
}
