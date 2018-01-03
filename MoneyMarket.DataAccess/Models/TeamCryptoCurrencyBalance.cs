using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.DataAccess.Models
{
    public class TeamCryptoCurrencyBalance : EntityBase
    {
        [Required]
        public int TeamId { get; set; } // foreign key 
        public virtual Team Team { get; set; } // navigation 

        [Required]
        public int CryptoCurrencyId { get; set; } // foreign key 
        public virtual CryptoCurrency CryptoCurrency { get; set; } // navigation 

        /// <summary>
        /// balance name
        /// </summary>
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public decimal Balance { get; set; }
    }
}
