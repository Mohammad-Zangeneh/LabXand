using LabXand.SharedKernel;
using System.Linq.Expressions;

namespace LabXand.Data
{
    public class ManyToManyUpdateConfiguration<TRoot, T, I> : UpdateConfigarationBase<TRoot>
        where TRoot : class
        where T : class, IEntity<I>
        where I : struct
    {
        public ManyToManyUpdateConfiguration(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            : base(propertyUpdaterCustomizer, constantFields)
        {
            InnerConfigurations = new List<object>();
        }
        public ManyToManyUpdateConfiguration()
            : this(new EmptyPropertyUpdaterCustomizer<TRoot>(), [])
        {
        }
        public Expression<Func<TRoot, IList<T>>> ItemSelector { get; set; }
    }
}
