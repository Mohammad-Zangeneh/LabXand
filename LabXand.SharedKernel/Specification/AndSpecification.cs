using System.Linq.Expressions;

namespace LabXand.SharedKernel
{
    public class AndSpecification<T, TIdentifier>(ISpecification<T, TIdentifier> specification1, ISpecification<T, TIdentifier> specification2) : CompositeSpecificationBase<T, TIdentifier>(specification1, specification2)
        where T : IEntity<TIdentifier>
        where TIdentifier : struct
    {
        protected override BinaryExpression CombineExpression(Expression body1, Expression body2) => Expression.AndAlso(body1, body2);
    }
}