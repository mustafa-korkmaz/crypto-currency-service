using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.Common.ApiObjects.Request.Product
{
    /// <summary>
    /// products search request
    /// </summary>
    public class SearchProductRequest
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
        [StringLength(50, ErrorMessage = "{0} max {1} karakter olmalıdır.")]
        [Display(Name = "Arama metni")]
        public string SearchText { get; set; }

        public bool? IsRelational { get; set; }
    }
}
