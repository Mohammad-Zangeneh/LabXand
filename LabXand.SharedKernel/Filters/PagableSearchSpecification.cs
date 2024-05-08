namespace LabXand.SharedKernel.Filters;

public class PagableSearchSpecification<TSearchModel> : SearchSpecification<TSearchModel>
    where TSearchModel : ISearchModel, new()
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}
