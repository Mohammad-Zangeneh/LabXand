using LabXand.SharedKernel;
using System.Linq.Expressions;

namespace LabXand.Data
{
    public class OneToOneUpdateConfiguration<TRoot, T, I> : UpdateConfigarationBase<TRoot>
        where TRoot : class
        where T : class, IEntity<I>
        where I : struct
    {
        public OneToOneUpdateConfiguration(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            : base(propertyUpdaterCustomizer, constantFields)
        {
            InnerConfigurations = new List<object>();
        }
        public OneToOneUpdateConfiguration()
            : this(new EmptyPropertyUpdaterCustomizer<TRoot>(), new List<string>())
        {
        }
        public Expression<Func<TRoot, T>> ItemSelector { get; set; }
    }
}
