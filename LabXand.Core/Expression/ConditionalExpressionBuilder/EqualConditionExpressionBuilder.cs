using System.Linq.Expressions;

namespace LabXand.Core
{
    public class EqualConditionExpressionBuilder : ConditionalExpressionBuilderBase
    {
        protected override BinaryExpression GetExpression(Expression leftExpression, Expression rightExpression)
        {
            return Expression.Equal(leftExpression, rightExpression);
        }
    }
}
