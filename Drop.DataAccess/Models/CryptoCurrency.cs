using System;
using System.ComponentModel.DataAnnotations;
using MoneyMarket.Common;

namespace MoneyMarket.DataAccess.Models
{
    /// <summary>
    /// dto.CryptoCurrency
    /// </summary>
    public class CryptoCurrency : EntityBase
    {
        public Currency Currency { get; set; }

        public Provider Provider { get; set; }

        /// <summary>
        /// refresher class name
        /// </summary>
        [MaxLength(20)]
        [Required]
        public string ClassName { get; set; }

        /// <summary>
        /// ticker.last
        /// </summary>
        public decimal UsdValue { get; set; }

        [Required]
        public DateTime ModifiedAt { get; set; }
    }
}
