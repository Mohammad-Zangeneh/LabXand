using System.Collections;

namespace LabXand.Data
{
    public interface IUpdateConfiguration<TRoot>
        where TRoot : class
    {
        List<string> ConstantFields { get; set; }
        IEnumerable InnerConfigurations { get; set; }
        INavigationPropertyUpdaterCustomizer<TRoot> PropertyUpdaterCustomizer { get; }
        void AddInnerConfigurations<T>(IUpdateConfiguration<T> innerConfiguration) where T : class;
    }
}
