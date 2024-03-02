namespace LabXand.Core
{
    public interface IPagedList<T> : IList<T>
    {
        int Index { get; set; }
        int Size { get; set; }
        int TotalCount { get; set; }
        /// <summary>
        /// The 1-based current page index
        /// </summary>
        int PageNumber { get; set; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
        int FirstItemIndex { get; }
        /// <summary>
        /// The 1-based index of the last item in the page.
        /// </summary>
        int LastItemIndex { get; }
        bool IsFirstPage { get; }
        bool IsLastPage { get; }
    }
}