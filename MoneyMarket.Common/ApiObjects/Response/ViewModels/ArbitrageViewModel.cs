using System;
using System.Collections.Generic;
using MoneyMarket.Common.Helper;

namespace MoneyMarket.Common.ApiObjects.Response.ViewModels
{
    /// <summary>
    /// ArbitrageViewModel
    /// </summary>
    public class ArbitrageViewModel
    {
        public IEnumerable<Arbitrage> Arbitrages { get; set; }

        private string _usdSellRate;
        public string UsdSellRate
        {
            get => $"{_usdSellRate} TRY";
            set => _usdSellRate = value;
        }
    }

    public class Arbitrage
    {
        public string Name { get; set; }
        public string Currency { get; set; }

        private string _diff;

        public string Diff
        {
            get => $"{_diff} USD";
            set => _diff = value;
        }
    }
}