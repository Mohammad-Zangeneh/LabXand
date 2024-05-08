using LabXand.Extensions;
using System.Linq.Expressions;

namespace LabXand.Core;

[Serializable]
public class NotEqualCriteria : Criteria
{
    protected override Expression CreateExpression(ParameterExpression parameter)
    {
        return ExpressionHelper.CreateConditionalExpression(parameter, this.FirstOprand.ToString(), ObjectType, this.SecondOperand, this.SecondOperand.GetType(), new NotEqualConditionExpressionBuilder());
    }
}
