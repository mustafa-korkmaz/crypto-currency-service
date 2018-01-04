using MoneyMarket.Common;

namespace MoneyMarket.Dto
{
    public class TeamCryptoCurrencyBalance : DtoBase
    {
        public int TeamId { get; set; } // foreign key 
        public virtual Team Team { get; set; } // navigation 

        public Currency Currency { get; set; } // foreign key 

        /// <summary>
        /// Unique balance name
        /// </summary>
        public string Name { get; set; }

        public decimal Balance { get; set; }
    }
}
