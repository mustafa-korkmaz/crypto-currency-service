
using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.Common.ApiObjects.Request.Common
{
    public class SendingNotificationRequest
    {
        [StringLength(100, ErrorMessage = "{0} alanı max {1} karakter olmalıdır.")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [Display(Name = "Bildirim İçeriği")]
        public string Content { get; set; }
    }

}