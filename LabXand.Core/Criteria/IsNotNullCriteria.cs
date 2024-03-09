using LabXand.Extensions;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LabXand.Core
{
    [Serializable]
    public class IsNotNullCriteria : Criteria
    {
        protected override Expression CreateExpression(ParameterExpression parameter)
        {
            return ExpressionHelper.CreateConditionalExpression(parameter, this.FirstOprand.ToString(), ObjectType, null, typeof(object), new IsNotNullConditionExpressionBuilder());
        }
    }
}
