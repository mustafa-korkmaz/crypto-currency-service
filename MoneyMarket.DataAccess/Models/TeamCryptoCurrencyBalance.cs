using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MoneyMarket.Common;

namespace MoneyMarket.DataAccess.Models
{
    public class TeamCryptoCurrencyBalance : EntityBase
    {
        [Required]
        public int TeamId { get; set; } // foreign key 
        public virtual Team Team { get; set; } // navigation 

        [Required]
        public Currency Currency { get; set; } // foreign key 

        /// <summary>
        /// balance name
        /// </summary>
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public decimal Balance { get; set; }
    }
}
