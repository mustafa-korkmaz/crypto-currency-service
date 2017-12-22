using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.Common.ApiObjects.Request.Alarm
{
    public class ListAlarmRequest
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}. ")]
        public int UserProductId { get; set; }
    }
}