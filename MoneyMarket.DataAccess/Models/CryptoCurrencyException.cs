using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.DataAccess.Models
{
    /// <summary>
    /// dbo.DropException
    /// </summary>
    public class CryptoCurrencyException : EntityBase
    {
        [MaxLength(150)]
        [Required]
        public string Message { get; set; }

        [Required]
        public string StackTrace { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
