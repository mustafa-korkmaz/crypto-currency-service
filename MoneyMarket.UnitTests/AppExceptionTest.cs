using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyMarket.Business.CryptoCurrency.Tickers.CoinMarketCap;
using MoneyMarket.Business.Exception;
namespace MoneyMarket.UnitTests
{
    [TestClass]
    public class AppExceptionTest
    {
        [TestMethod]
        [AppExceptionAttribute]
        public void Catch_ApplicationException()
        {
            var resp = new All().GetCurrentCryptoCurrency();
        }
    }
}
