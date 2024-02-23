using System.Collections;

namespace LabXand.Data
{
    public abstract class UpdateConfigarationBase<TRoot> : IUpdateConfiguration<TRoot>
        where TRoot : class
    {
        protected UpdateConfigarationBase(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer,List<string> constantFields)
        {
            _propertyUpdaterCustomizer = propertyUpdaterCustomizer;
            ConstantFields = constantFields;
        }
        public IEnumerable InnerConfigurations { get; set; }
        INavigationPropertyUpdaterCustomizer<TRoot> _propertyUpdaterCustomizer;
        public INavigationPropertyUpdaterCustomizer<TRoot> PropertyUpdaterCustomizer { get { return _propertyUpdaterCustomizer; } }

        public List<string> ConstantFields { get; set; }
        
        public void AddInnerConfigurations(IUpdateConfiguration<object> innerConfiguration)
        {
            if (InnerConfigurations == null)
                InnerConfigurations = new List<object>();
            ((List<object>)InnerConfigurations).Add(innerConfiguration);
        }

        public abstract INavigationPropertyUpdater<TRoot> CreateUpdater(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer);

    }
}
