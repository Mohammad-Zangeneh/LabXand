using LabXand.Extensions;
using System.Linq.Expressions;

namespace LabXand.SharedKernel
{
    public abstract class CompositeSpecificationBase<T, TIdentifier>(ISpecification<T, TIdentifier> specification1, ISpecification<T, TIdentifier> specification2) : SpecificationBase<T, TIdentifier>
        where T : IEntity<TIdentifier>
        where TIdentifier : struct
    {
        protected readonly ISpecification<T, TIdentifier> Specification1 = specification1;

        protected readonly ISpecification<T, TIdentifier> Specification2 = specification2;

        public override Expression<Func<T, bool>> Criteria
        {
            get
            {
                var parameter = Expression.Parameter(typeof(T), "x");

                var body1 = new PredicateRewriterVisitor(parameter).Visit(Specification1.Criteria.Body);
                var body2 = new PredicateRewriterVisitor(parameter).Visit(Specification2.Criteria.Body);
                BinaryExpression combinedBody = CombineExpression(body1, body2);

                return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
            }
        }

        protected abstract BinaryExpression CombineExpression(Expression body1, Expression body2);
    }
}