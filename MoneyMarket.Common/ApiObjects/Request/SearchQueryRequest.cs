using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.Common.ApiObjects.Request
{
    /// <summary>
    ///  search query request object 
    /// </summary>
    public class SearchQueryRequest
    {
        /// <summary>
        /// desired start index for PagedList items
        /// </summary>
        ///    [Required]
        [Required]
        [Range(0, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}. ")]
        public int Start { get; set; }

        /// <summary>
        /// desired length of PagedList items
        /// </summary>
        [Required]
        [Range(1, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}. ")]
        public int Length { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Value for {0} must be between {1} and {2}. ")]
        public int SearchType { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Value for {0} must be at least {1} char. ")]
        public string SearchText { get; set; }
    }
}