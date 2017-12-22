using System;
using MoneyMarket.Common;

namespace MoneyMarket.Dto
{
    /// <summary>
    /// dto.CryptoCurrency
    /// </summary>
    public class CryptoCurrency : DtoBase
    {
        public Currency Currency { get; set; }

        public Provider Provider { get; set; }

        /// <summary>
        /// refresher class name
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// ticker.last
        /// </summary>
        public decimal UsdValue { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}
