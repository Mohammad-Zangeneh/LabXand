using LabXand.Extensions;
using System.Linq.Expressions;

namespace LabXand.Core;

[Serializable]
public class LessThanOrEqual : Criteria
{
    protected override Expression CreateExpression(ParameterExpression parameter)
    {
        return ExpressionHelper.CreateConditionalExpression(parameter, this.FirstOprand.ToString(), ObjectType, this.SecondOperand, this.SecondOperand.GetType(), new LessThanOrEqualConditionExpressionBuilder());
    }
}
