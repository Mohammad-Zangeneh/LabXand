using System.Linq.Expressions;

namespace LabXand.Core
{
    public class GreaterThanConditionExpressionBuilder : ConditionalExpressionBuilderBase
    {
        protected override BinaryExpression GetExpression(Expression leftExpression, Expression rightExpression)
        {
            return Expression.GreaterThan(leftExpression, rightExpression);
        }
    }
}
