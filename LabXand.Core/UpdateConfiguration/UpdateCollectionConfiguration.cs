using LabXand.SharedKernel;
using System.Linq.Expressions;

namespace LabXand.Data
{
    public class UpdateCollectionConfiguration<TRoot, T, I> : UpdateConfigarationBase<TRoot>
        where TRoot : class
        where T : class, IEntity<I>
        where I : struct
    {
        public UpdateCollectionConfiguration(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            : base(propertyUpdaterCustomizer, constantFields)
        {
            InnerConfigurations = new List<object>();
        }

        public UpdateCollectionConfiguration(List<string> constantFields)
            : base(new EmptyPropertyUpdaterCustomizer<TRoot>(), constantFields)
        {
            InnerConfigurations = new List<object>();
        }

        public UpdateCollectionConfiguration()
            : this(new List<string>())
        {
        }

        public Expression<Func<TRoot, ICollection<T>>> ItemSelector { get; set; }
        public string ParentPropertyName { get; set; }
        public string ChildtPropertyName { get; set; }
        public override INavigationPropertyUpdater<TRoot> CreateUpdater(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
        {
            return new CollectionNavigationPropertyUpdater<TRoot, T, I>(propertyUpdaterCustomizer, this);
        }
    }
}
