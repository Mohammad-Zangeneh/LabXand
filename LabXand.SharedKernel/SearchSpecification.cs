using LabXand.Core;

namespace LabXand.SharedKernel;

public class SearchSpecification<TSearchModel>
    where TSearchModel : ISearchModel
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public List<SortItem> SortItems { get; set; } = [];
    public TSearchModel FilterValues { get; set; }
    public Criteria GetCriteria() => FilterValues.GetCriteria();
}
