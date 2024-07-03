using System.Linq.Expressions;

namespace LabXand.SharedKernel
{
    public interface ISpecification<T, TIdentifier>
        where T : IEntity<TIdentifier>
        where TIdentifier : struct
    {
        Expression<Func<T, bool>> Criteria { get; }
        bool IsSatisfiedBy(T domain);
    }
}