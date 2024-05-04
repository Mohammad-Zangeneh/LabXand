using LabXand.SharedKernel;
using System.Linq.Expressions;

namespace LabXand.Data
{
    public class OneToOneUpdateConfiguration<TRoot, T, I>(Expression<Func<TRoot, T>> itemSelector, List<string> constantFields) : UpdateConfigarationBase<TRoot>(constantFields)
        where TRoot : class
        where T : class, IEntity<I>
        where I : struct
    {
        public OneToOneUpdateConfiguration(Expression<Func<TRoot, T>> itemSelector)
            : this(itemSelector, [])
        {
        }
        public Expression<Func<TRoot, T>> ItemSelector { get; set; } = itemSelector;
    }
}
