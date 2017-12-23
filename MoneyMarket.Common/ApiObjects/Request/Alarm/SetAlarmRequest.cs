using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.Common.ApiObjects.Request.Alarm
{
    public class SetAlarmRequest
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}. ")]
        public int UserProductId { get; set; }

        public decimal ExactValue { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Value for {0} must be between {1} and {2}. ")]
        public int SettingId { get; set; }
    }
}