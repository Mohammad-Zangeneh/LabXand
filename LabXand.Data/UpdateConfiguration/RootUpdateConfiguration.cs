namespace LabXand.Data
{
    public class RootUpdateConfiguration<TRoot> : UpdateConfigarationBase<TRoot>
        where TRoot : class
    {
        public RootUpdateConfiguration(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            : base(propertyUpdaterCustomizer, constantFields)
        {
            InnerConfigurations = new List<object>();
        }
        public RootUpdateConfiguration(List<string> constantFields)
            : this(new EmptyPropertyUpdaterCustomizer<TRoot>(), constantFields)
        {

        }
    }
}
