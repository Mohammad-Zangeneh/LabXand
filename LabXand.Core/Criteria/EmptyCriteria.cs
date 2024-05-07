using System.Linq.Expressions;

namespace LabXand.Core;

public class EmptyCriteria : Criteria
{
    protected override void Initialize(ParameterExpression parameter)
    {

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
