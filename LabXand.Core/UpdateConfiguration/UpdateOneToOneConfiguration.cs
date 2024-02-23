using LabXand.SharedKernel;
using System.Linq.Expressions;

namespace LabXand.Data
{
    public class UpdateOneEntityConfiguration<TRoot, T, I> : UpdateConfigarationBase<TRoot>
        where TRoot : class
        where T : class, IEntity<I>
        where I : struct
    {
        public UpdateOneEntityConfiguration(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            : base(propertyUpdaterCustomizer, constantFields)
        {
            InnerConfigurations = new List<object>();
        }
        public UpdateOneEntityConfiguration()
            : this(new EmptyPropertyUpdaterCustomizer<TRoot>(), new List<string>())
        {
        }
        public Expression<Func<TRoot, T>> ItemSelector { get; set; }
        public string ParentPropertyName { get; set; }
        public string ChildtPropertyName { get; set; }
        public override INavigationPropertyUpdater<TRoot> CreateUpdater(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
        {
            return new OneNavigationPropertyUpdater<TRoot, T, I>(propertyUpdaterCustomizer, this);
        }
    }
}
