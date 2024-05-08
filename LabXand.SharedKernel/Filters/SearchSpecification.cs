using LabXand.Core;

namespace LabXand.SharedKernel.Filters;

public class SearchSpecification<TSearchModel>
    where TSearchModel : ISearchModel, new()
{
    public List<SortItem> SortItems { get; set; } = [];
    public TSearchModel FilterValues { get; set; } = new();
    public Criteria GetCriteria() => FilterValues.GetCriteria();
}
