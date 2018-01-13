using System;
using System.Collections.Generic;
using System.Linq;
using MoneyMarket.Business.HttpClient;
using MoneyMarket.Common;
using MoneyMarket.Common.Helper;
using Newtonsoft.Json;

namespace MoneyMarket.Business.CryptoCurrency.Tickers.CoinMarketCap
{
    public class Ignis : ITicker
    {
        public decimal UsdSellRate { get; set; }

        public IEnumerable<Dto.CryptoCurrency> GetCurrentCryptoCurrency()
        {
            var apiClient = new ApiClient();

            var resp = apiClient.GetWebResponse("https://api.coinmarketcap.com/v1/ticker/Ignis/");

            var tickers = JsonConvert.DeserializeObject<IEnumerable<JsonTicker>>(resp.ResponseData);

            var list = new List<Dto.CryptoCurrency>();
            var now = DateTime.UtcNow;

            foreach (var ticker in tickers)
            {
                var currency = new Dto.CryptoCurrency
                {
                    UsdValue = ticker.price_usd,
                    Currency = Currency.Ignis,
                    Provider = Provider.CoinMarketCap,
                    ModifiedAt = now
                };

                list.Add(currency);
            }

            return list.Where(p => p.Currency != Currency.Unknown);
        }

    }
}
