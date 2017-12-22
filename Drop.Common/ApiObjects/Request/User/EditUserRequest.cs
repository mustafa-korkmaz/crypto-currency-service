using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.Common.ApiObjects.Request.User
{
    public class EditUserRequest
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} alanı max {1} karakter olmalıdır.")]
        [Display(Name = "Ad Soyad")]
        public string NameSurname { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}

