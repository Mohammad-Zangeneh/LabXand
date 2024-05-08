namespace LabXand.Core;

public class SpecificationOfDataList<T>
    where T : class
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public List<FilterSpecification<T>> FilterSpecifications { get; set; }
    public List<SortSpecification> SortSpecifications { get; set; }

    public Criteria GetCriteria()
    {
        Criteria? criteria = null;
        if (FilterSpecifications != null)
        {
            foreach (var item in FilterSpecifications)
            {
                if (criteria != null)
                    criteria = criteria.And(CriteriaBuilder.CreateFromFilterOperation<T>(item.FilterOperation, item.PropertyName, item.FilterValue));
                else
                    criteria = CriteriaBuilder.CreateFromFilterOperation<T>(item.FilterOperation, item.PropertyName, item.FilterValue);
            }
        }
        return criteria;
    }

    public List<SortItem> GetSortItem()
    {
        List<SortItem> sortItems = [];
        if (SortSpecifications != null && SortSpecifications.Count > 0)
        {
            foreach (var item in SortSpecifications)
            {
                sortItems.Add(new SortItem() { SortFiledsSelector = (!string.IsNullOrWhiteSpace(item.SortField)) ? item.SortField : "Id", Direction = item.AscendingSortDirection ? SortDirection.Ascending : SortDirection.Descending });
            }
        }
        else
            sortItems.Add(new SortItem() { SortFiledsSelector = "Id", Direction = SortDirection.Ascending });

        return sortItems;
    }
}
