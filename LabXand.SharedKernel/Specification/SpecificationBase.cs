using System.Linq.Expressions;

namespace LabXand.SharedKernel
{
    public abstract class SpecificationBase<T, TIdentifier> : ISpecification<T, TIdentifier>
        where T : IEntity<TIdentifier>
        where TIdentifier : struct
    {
        public abstract Expression<Func<T, bool>> Criteria { get; }

        public bool IsSatisfiedBy(T domain)
            => Criteria.Compile().Invoke(domain);
    }
}