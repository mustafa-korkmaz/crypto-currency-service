
using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.Common.ApiObjects.Request.User
{
    public class VerificationLinkRequest
    {
        [Display(Name = "Kullanıcı Adı ya da Email")]
        public string UserNameOrEmail { get; set; }

        [Required]
        [Display(Name = "Güvenlik Kodu")]
        public string SecurityCode { get; set; }
    }

}
