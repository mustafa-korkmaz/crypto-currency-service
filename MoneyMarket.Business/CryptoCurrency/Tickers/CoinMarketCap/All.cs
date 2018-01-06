using System.Collections.Generic;
using System.Linq;
using MoneyMarket.Business.HttpClient;
using MoneyMarket.Common;
using MoneyMarket.Common.Helper;
using Newtonsoft.Json;

namespace MoneyMarket.Business.CryptoCurrency.Tickers.CoinMarketCap
{
    public class All : ITicker
    {
        public decimal UsdSellRate { get; set; }

        public IEnumerable<Dto.CryptoCurrency> GetCurrentCryptoCurrency()
        {
            var apiClient = new ApiClient();

            var resp = apiClient.GetWebResponse("https://api.coinmarketcap.com/v1/ticker/");

            var tickers = JsonConvert.DeserializeObject<IEnumerable<JsonTicker>>(resp.ResponseData);

            var list = new List<Dto.CryptoCurrency>();
            var now = Statics.GetTurkeyCurrentDateTime();

            foreach (var ticker in tickers)
            {
                var currency = new Dto.CryptoCurrency
                {
                    UsdValue = ticker.price_usd,
                    Currency = GetCurrency(ticker.symbol),
                    Provider = Provider.CoinMarketCap,
                    ModifiedAt = now
                };

                list.Add(currency);
            }

            return list.Where(p => p.Currency != Currency.Unknown);
        }


        private Currency GetCurrency(string symbol)
        {
            switch (symbol)
            {
                case "BTC":
                    return Currency.Btc;
                case "ETH":
                    return Currency.Eth;
                case "ETC":
                    return Currency.Etc;
                case "EOS":
                    return Currency.Eos;
                case "XLM":
                    return Currency.Stellar;
                case "XRP":
                    return Currency.Ripple;
                case "XEM":
                    return Currency.Nem;
                case "NXT":
                    return Currency.Nxt;
                case "LSK":
                    return Currency.Lisk;
            }

            return Currency.Unknown;
        }
    }
}
