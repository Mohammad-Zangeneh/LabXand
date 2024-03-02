using LabXand.SharedKernel;
using System.Linq.Expressions;

namespace LabXand.Data
{
    public class OneToManyUpdateConfiguration<TRoot, T, I> : UpdateConfigarationBase<TRoot>
        where TRoot : class
        where T : class, IEntity<I>
        where I : struct
    {
        public OneToManyUpdateConfiguration(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            : base(propertyUpdaterCustomizer, constantFields) => InnerConfigurations = new List<object>();

        public OneToManyUpdateConfiguration(List<string> constantFields)
            : base(new EmptyPropertyUpdaterCustomizer<TRoot>(), constantFields) => InnerConfigurations = new List<object>();

        public OneToManyUpdateConfiguration()
            : this([])
        {
        }

        public Expression<Func<TRoot, ICollection<T>>> ItemSelector { get; set; }
    }
}
