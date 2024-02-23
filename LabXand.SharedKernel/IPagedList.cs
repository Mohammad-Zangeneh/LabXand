namespace LabXand.SharedKernel
{
    public interface IPagedList<T> : IList<T>
    {
        int Index { get; set; }

        /// <summary>
        /// The number of items in each page.
        /// </summary>
        int Size { get; set; }

        /// <summary>
        /// The total number of items.
        /// </summary>
        int TotalCount { get; set; }
    }
}