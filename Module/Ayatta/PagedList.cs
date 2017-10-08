using System.Linq;
using System.Collections.Generic;

namespace Ayatta
{
    /// <summary>
    /// PagedList Interface
    /// </summary>
    public interface IPagedList<T> : IList<T>
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalPages { get; }
        int TotalRecords { get; }
        bool HasPrevPage { get; }
        bool HasNextPage { get; }
    }

    /// <summary>
    /// PagedList
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalRecords { get; }
        public int TotalPages { get; }

        public bool HasPrevPage => (PageIndex - 1 > 0);

        public bool HasNextPage => (PageIndex < TotalPages);

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var total = source.Count();
            TotalRecords = total;
            TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IList<T> source, int pageIndex, int pageSize)
        {
            TotalRecords = source.Count();
            TotalPages = TotalRecords / pageSize;

            if (TotalRecords % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalRecords">Total Record</param>
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalRecords)
        {
            TotalRecords = totalRecords;
            TotalPages = TotalRecords / pageSize;

            if (TotalRecords % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source);
        }
    }
}