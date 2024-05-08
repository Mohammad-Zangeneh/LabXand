using LabXand.Extensions;
using System.Linq.Expressions;

namespace LabXand.Core;

[Serializable]
public class IsNullCriteria : Criteria
{
    protected override Expression CreateExpression(ParameterExpression parameter)
    {
        return ExpressionHelper.CreateConditionalExpression(parameter, this.FirstOprand.ToString(), ObjectType, null, typeof(object), new IsNullConditionExpressionBuilder());
    }
}
