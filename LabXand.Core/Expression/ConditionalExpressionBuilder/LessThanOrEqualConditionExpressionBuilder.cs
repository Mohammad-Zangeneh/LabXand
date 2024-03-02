using System.Linq.Expressions;

namespace LabXand.Core
{
    public class LessThanOrEqualConditionExpressionBuilder : ConditionalExpressionBuilderBase
    {
        protected override BinaryExpression GetExpression(Expression leftExpression, Expression rightExpression)
        {
            return Expression.LessThanOrEqual(leftExpression, rightExpression);
        }
    }
}
