using System.Linq.Expressions;

namespace LabXand.Core
{
    public class GreaterThanOrEqualConditionExpressionBuilder : ConditionalExpressionBuilderBase
    {
        protected override BinaryExpression GetExpression(Expression leftExpression, Expression rightExpression)
        {
            return Expression.GreaterThanOrEqual(leftExpression, rightExpression);
        }
    }
}
