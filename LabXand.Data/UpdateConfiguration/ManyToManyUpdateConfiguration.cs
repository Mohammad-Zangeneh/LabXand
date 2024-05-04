using LabXand.SharedKernel;
using System.Linq.Expressions;

namespace LabXand.Data
{
    public class ManyToManyUpdateConfiguration<TRoot, T, I>(Expression<Func<TRoot, IList<T>>> itemSelectors,List<string> constantFields) : UpdateConfigarationBase<TRoot>(constantFields)
        where TRoot : class
        where T : class, IEntity<I>
        where I : struct
    {
        public ManyToManyUpdateConfiguration(Expression<Func<TRoot, IList<T>>> itemSelectors)
            : this(itemSelectors, [])
        {
        }
        public Expression<Func<TRoot, IList<T>>> ItemSelector { get; set; } = itemSelectors;
    }
}
