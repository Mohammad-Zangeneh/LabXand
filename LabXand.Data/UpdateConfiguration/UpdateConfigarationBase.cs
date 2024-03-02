using System.Collections;

namespace LabXand.Data
{
    public abstract class UpdateConfigarationBase<TRoot> : IUpdateConfiguration<TRoot>
        where TRoot : class
    {
        protected UpdateConfigarationBase(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer,List<string> constantFields)
        {
            this.propertyUpdaterCustomizer = propertyUpdaterCustomizer;
            ConstantFields = constantFields;
        }
        public IEnumerable InnerConfigurations { get; set; }
        readonly INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer;
        public INavigationPropertyUpdaterCustomizer<TRoot> PropertyUpdaterCustomizer { get { return propertyUpdaterCustomizer; } }

        public List<string> ConstantFields { get; set; }
        
        public void AddInnerConfigurations<T>(IUpdateConfiguration<T> innerConfiguration) where T : class
        {
            if (InnerConfigurations == null)
                InnerConfigurations = new List<object>();
            ((List<object>)InnerConfigurations).Add(innerConfiguration);
        }

    }
}
