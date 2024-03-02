using Ardalis.GuardClauses;

namespace LabXand.Core
{
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {

            Guard.Against.Null(source);
            Init(source.Skip(pageIndex * pageSize).Take(pageSize), pageIndex, pageSize, source.Count());
        }
        public PagedList(ICollection<T> source, int pageIndex, int pageSize)
        {

            Guard.Against.Null(source);
            Init(source.Skip(pageIndex * pageSize).Take(pageSize), pageIndex, pageSize, source.Count);
        }
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {

            Guard.Against.Null(source);
            Init(source, pageIndex, pageSize, totalCount);
        }

        private void Init(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            Guard.Against.Negative(pageIndex);
            Guard.Against.NegativeOrZero(pageSize);

            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;

            AddRange(source);
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber
        {
            get
            {
                return PageIndex + 1;
            }
            set
            {
                PageIndex = value - 1;
            }
        }

        public int TotalPages
        {
            get
            {
                var total = TotalCount / PageSize;

                if (TotalCount % PageSize > 0)
                    total++;

                return total;
            }
        }

        public bool HasPreviousPage => PageIndex > 0;

        public bool HasNextPage => (PageIndex < (TotalPages - 1));

        public int FirstItemIndex => (PageIndex * PageSize) + 1;

        public int LastItemIndex => Math.Min(TotalCount, ((PageIndex * PageSize) + PageSize));

        public bool IsFirstPage => (PageIndex <= 0);

        public bool IsLastPage => (PageIndex >= TotalPages);

        public int Index { get; set; }
        public int Size { get; set; }
    }
}