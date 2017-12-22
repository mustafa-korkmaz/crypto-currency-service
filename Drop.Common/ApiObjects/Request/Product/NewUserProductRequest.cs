using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.Common.ApiObjects.Request.Product
{
    public class NewUserProductRequest
    {
        [StringLength(500, ErrorMessage = "{0} alanı max {1} karakter olmalıdır.")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [Display(Name = "Ürün Linki")]
        public string Url { get; set; }
    }
}
