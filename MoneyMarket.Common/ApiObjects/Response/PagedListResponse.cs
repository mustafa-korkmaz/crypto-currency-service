using System.Collections.Generic;
using System.Linq;

namespace MoneyMarket.Common.ApiObjects.Response
{
    /// <summary>
    /// paged list which returned by betblogger api methods
    /// </summary>
    /// <typeparam name="T"> api paged list generic</typeparam>
    public class PagedListResponse<T>
    {
        /// <summary>
        /// Paged list items
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// Returns if the result in the first page or not
        /// </summary>
        public bool IsFirstPage => Start == 0;

        /// <summary>
        /// Returns if the result in the last page or not
        /// </summary>
        public bool IsLastPage
        {
            get
            {
                if (Items == null)
                {
                    return true;
                }

                return Items.Count() < Length;
            }
        }

        public int Start { private get; set; }
        public int Length { private get; set; }
        public int TotalCount { get; set; }
    }

}
