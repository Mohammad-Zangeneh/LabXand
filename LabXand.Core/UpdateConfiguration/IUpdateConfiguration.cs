using System.Collections;

namespace LabXand.Data
{
    public interface IUpdateConfiguration<TRoot>
        where TRoot : class
    {
        IEnumerable InnerConfigurations { get; set; }
        INavigationPropertyUpdaterCustomizer<TRoot> PropertyUpdaterCustomizer { get; }
        void AddInnerConfigurations<T>(IUpdateConfiguration<T> innerConfiguration) where T : class;
    }
}
