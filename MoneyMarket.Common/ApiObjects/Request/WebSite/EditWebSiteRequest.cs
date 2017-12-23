
using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.Common.ApiObjects.Request.WebSite
{
    public class EditWebSiteRequest
    {
        [StringLength(50, ErrorMessage = "{0} alanı max {1} karakter olmalıdır.")]
        [Display(Name = "Site Adı")]
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "{0} alanı max {1} karakter olmalıdır.")]
        [Display(Name = "Site Link")]
        public string Domain { get; set; } // we dont want duplicated urls

        [StringLength(100, ErrorMessage = "{0} alanı max {1} karakter olmalıdır.")]
        public string ImageUrl { get; set; }

        public int TrackedProductTotal { get; set; } // a shortcut to web site's tracked product count
    }

}

