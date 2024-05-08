using System.Linq.Expressions;

namespace LabXand.Core;

public class SortItem
{
    public SortDirection Direction { get; set; }
    public string SortFiledsSelector { get; set; }
}
