using System.Collections.Generic;

namespace MoneyMarket.Business.CryptoCurrency.Tickers
{
    interface ITicker
    {
        IEnumerable<Dto.CryptoCurrency> GetCurrentCryptoCurrency();

        decimal UsdSellRate { get; set; }
    }
}
