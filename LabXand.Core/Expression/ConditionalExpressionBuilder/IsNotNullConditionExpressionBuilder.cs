using System.Linq.Expressions;
using System.Reflection;

namespace LabXand.Core
{
    public class IsNotNullConditionExpressionBuilder : IConditionalExpressionBuilder
    {
        public Expression Get(MemberExpression memberExpression, object value, Type valueType)
        {
            Type firstdOperandType = ((PropertyInfo)(memberExpression.Member)).PropertyType;
            return Expression.NotEqual(memberExpression, Expression.Constant(null));            
        }
    }
}
