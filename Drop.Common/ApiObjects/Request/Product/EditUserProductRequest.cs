using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.Common.ApiObjects.Request.Product
{
    public class EditUserProductRequest
    {
        [StringLength(100, ErrorMessage = "{0} alanı max {1} karakter olmalıdır.")]
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; }

        /// <summary>
        /// send 10 for not modify
        /// </summary>
        [Required]
        public int Status { get; set; }
    }
}
