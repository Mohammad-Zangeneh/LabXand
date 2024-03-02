using System.Linq.Expressions;

namespace LabXand.Core
{
    public interface IConditionalExpressionBuilder
    {
        Expression Get(MemberExpression memberExpression, object value, Type valueType);
    }
}
