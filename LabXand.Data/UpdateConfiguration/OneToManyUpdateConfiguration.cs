using LabXand.SharedKernel;
using System.Linq.Expressions;

namespace LabXand.Data
{
    public class OneToManyUpdateConfiguration<TRoot, T, I>(Expression<Func<TRoot, IList<T>>> itemSelector, List<string> constantFields) : UpdateConfigarationBase<TRoot>(constantFields)
        where TRoot : class
        where T : class, IEntity<I>
        where I : struct
    {
        public OneToManyUpdateConfiguration(Expression<Func<TRoot, IList<T>>> itemSelector)
            : this(itemSelector, []) { }

        public Expression<Func<TRoot, IList<T>>> ItemSelector { get; set; } = itemSelector;
    }
}
