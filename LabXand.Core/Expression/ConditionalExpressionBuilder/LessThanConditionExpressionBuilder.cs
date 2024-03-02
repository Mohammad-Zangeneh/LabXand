using System.Linq.Expressions;

namespace LabXand.Core
{
    public class LessThanConditionExpressionBuilder : ConditionalExpressionBuilderBase
    {
        protected override BinaryExpression GetExpression(Expression leftExpression, Expression rightExpression)
        {
            return Expression.LessThan(leftExpression, rightExpression);
        }
    }
}
