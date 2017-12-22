using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyMarket.Business.CryptoCurrency.Tickers.CoinMarketCap;

namespace MoneyMarket.UnitTests
{
    [TestClass]
    public class TickerTest
    {
        [TestMethod]
        public void Get_CoinMarketCap_Ticker()
        {
            var resp = new All().GetCurrentCryptoCurrency();
        }
    }
}
