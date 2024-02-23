using LabXand.SharedKernel;
using System.Linq.Expressions;

namespace LabXand.Data
{
    public class UpdateManyToManyCollection<TRoot, T, I> : UpdateConfigarationBase<TRoot>
        where TRoot : class
        where T : class, IEntity<I>
        where I : struct
    {
        public UpdateManyToManyCollection(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            : base(propertyUpdaterCustomizer, constantFields)
        {
            InnerConfigurations = new List<object>();
        }
        public UpdateManyToManyCollection()
            : this(new EmptyPropertyUpdaterCustomizer<TRoot>(), new List<string>())
        {
        }
        public Expression<Func<TRoot, ICollection<T>>> ItemSelector { get; set; }

        public override INavigationPropertyUpdater<TRoot> CreateUpdater(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
        {
            return new ManyToManyUpdater<TRoot, T, I>(propertyUpdaterCustomizer, this);
        }
    }
}
