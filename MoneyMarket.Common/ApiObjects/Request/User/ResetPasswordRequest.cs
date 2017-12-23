using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.Common.ApiObjects.Request.User
{
    public class ResetPasswordRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} alanı en az {2} karakter olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifreyi doğrula")]
        [Compare("NewPassword", ErrorMessage = "Girilen şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }

        public string UserId { get; set; }
    }

    public class ChangePasswordRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} alanı en az {2} karakter olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifreyi doğrula")]
        [Compare("NewPassword", ErrorMessage = "Girilen şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} alanı en az {2} karakter olmalıdır.", MinimumLength = 6)]
        [Display(Name = "Mevcut Şifre")]
        public string CurrentPassword { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
