using System.Linq.Expressions;

namespace LabXand.Core;

public class EmptyCriteria : Criteria
{
    public override Expression GetExpression(ParameterExpression parameter)
    {
        return CreateExpression(parameter);
    }

    protected override Expression CreateExpression(ParameterExpression parameter)
    {
        if (parameter == null)
        {
            parameter = Expression.Parameter(ObjectType);
        }

        return Expression.Constant(true);
    }
}
