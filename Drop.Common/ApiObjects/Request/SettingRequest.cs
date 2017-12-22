using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.Common.ApiObjects.Request
{
    /// <summary>
    /// user setting request objects
    /// </summary>
    public class AuthorNotificationRequest
    {
        /// <summary>
        /// desired start index for PagedList items
        /// </summary>
        ///    [Required]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be greater than 0. ")]
        public int AuthorId { get; set; }

        [Required]
        public bool Value { get; set; }
    }
}