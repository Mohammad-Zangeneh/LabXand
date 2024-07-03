using System.Linq.Expressions;

namespace LabXand.SharedKernel
{
    public class OrSpecification<T, TIdentifier>(ISpecification<T, TIdentifier> specification1, ISpecification<T, TIdentifier> specification2) : CompositeSpecificationBase<T, TIdentifier>(specification1, specification2)
        where T : IEntity<TIdentifier>
        where TIdentifier : struct
    {
        protected override BinaryExpression CombineExpression(Expression body1, Expression body2) => Expression.OrElse(body1, body2);
    }
}