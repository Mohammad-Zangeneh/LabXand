using System.Linq.Expressions;

namespace LabXand.Core
{
    public class NotEqualConditionExpressionBuilder : ConditionalExpressionBuilderBase
    {
        protected override BinaryExpression GetExpression(Expression leftExpression, Expression rightExpression)
        {
            return Expression.NotEqual(leftExpression, rightExpression);
        }
    }
}
