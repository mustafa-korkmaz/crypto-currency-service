using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.Api.Models
{
    // Models used as parameters to AccountController actions.

    public class LogoutModel
    {
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string DeviceKey { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} alanı en az {2} karakter olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifreyi doğrula")]
        [Compare("NewPassword", ErrorMessage = "Girilen şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// User registration model class
    /// </summary>
    public class RegisterBindingModel
    {
        [Display(Name = "Email")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Geçerli bir email adresi giriniz.")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string Email { get; set; }

        [Display(Name = "İsim Soy isim")]
        public string NameSurname { get; set; }

        //public string DeviceKey { get; set; }

        //public bool ContactPermission { get; set; }

        //[RegularExpression(@"\b\d{3}?\d{3}?\d{4}\b", ErrorMessage = "Geçerli bir telefon numarası giriniz.(5441234567)")]
        //[Display(Name = "Telefon")]
        //public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "{0} alanı için sadece alfa numerik ve alt çizgi karakteri girebilirisiniz.")]
        [StringLength(15, ErrorMessage = "{0} alanı en az {2} en çok {1} karakter olmalıdır.", MinimumLength = 4)]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "{0} alanı en az {2} en çok {1} karakter olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }
    }

    public class SetPasswordBindingModel
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
    }

    public class RemindPaswordModel
    {
        [StringLength(100, ErrorMessage = "Email ya da kullanıcı adı alanı en az {2} karakter olmalıdır.", MinimumLength = 4)]
        [Required]
        public string EmailOrUserName { get; set; }
    }

    public class UserDeviceModel
    {
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string DeviceKey { get; set; }
    }

    public class UserRequestModel
    {
        [StringLength(250, ErrorMessage = "Mesaj alanı en az {2} en çok {1} karakter olmalıdır.", MinimumLength = 4)]
        [Required]
        public string Message { get; set; }

        [Range(0, 3, ErrorMessage = "Value for {0} must be between {1} and {2}. ")]
        [Required]
        public int Type { get; set; }
    }
}