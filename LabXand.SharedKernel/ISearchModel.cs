using LabXand.Core;

namespace LabXand.SharedKernel;

public interface ISearchModel
{
    void AddMapConfig(string propertyName, FilterOperations operation, string domainProperty);
    Criteria GetCriteria();
}
