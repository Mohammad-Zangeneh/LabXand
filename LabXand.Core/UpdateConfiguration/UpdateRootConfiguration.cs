namespace LabXand.Data
{
    public class UpdateRootConfiguration<TRoot> : UpdateConfigarationBase<TRoot>
        where TRoot : class
    {
        public UpdateRootConfiguration(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            : base(propertyUpdaterCustomizer, constantFields)
        {
            InnerConfigurations = new List<object>();
        }
        public UpdateRootConfiguration(List<string> constantFields)
            : this(new EmptyPropertyUpdaterCustomizer<TRoot>(), constantFields)
        {

        }

        public override INavigationPropertyUpdater<TRoot> CreateUpdater(INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
        {
            return new RootNavigationPropertyUpdater<TRoot>(propertyUpdaterCustomizer, this);
        }
    }
}
