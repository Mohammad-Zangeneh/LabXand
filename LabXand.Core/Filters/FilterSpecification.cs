using LabXand.Extensions;

namespace LabXand.Core
{
    public class FilterSpecification<T>
    {
        public string PropertyName { get; set; }
        object _filterValue;
        public object FilterValue
        {
            get { return _filterValue; }
            set
            {
                if (value is string)
                {
                    string str = (string)value;
                    _filterValue = str.ApplyCorrectYeKe();
                }
                else
                    _filterValue = value;
            }
        }
        public FilterOperations FilterOperation { get; set; }
    }
}
