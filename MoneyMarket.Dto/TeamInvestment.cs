using MoneyMarket.Common;

namespace MoneyMarket.Dto
{
    public class TeamInvestment : DtoBase
    {
        public int TeamId { get; set; } // foreign key 
        public virtual Team Team { get; set; } // navigation 

        public Currency Currency { get; set; } // foreign key 

        /// <summary>
        /// Unique balance name
        /// </summary>
        public string Name { get; set; }

        public decimal Balance { get; set; }

        ///// <summary>
        ///// not mapped value
        ///// </summary>
        //public decimal UnitUsdValue { private get; set; }

        ///// <summary>
        ///// balance * unit usd value
        ///// </summary>
        //public decimal TotalUsdValue => Balance * UnitUsdValue;
    }
}
