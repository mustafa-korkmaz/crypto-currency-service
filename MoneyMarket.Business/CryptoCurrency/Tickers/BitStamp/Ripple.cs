using System;
using System.Collections.Generic;
using MoneyMarket.Business.HttpClient;
using MoneyMarket.Common;
using Newtonsoft.Json;

namespace MoneyMarket.Business.CryptoCurrency.Tickers.BitStamp
{
    public class Ripple : ITicker
    {
        private Dto.CryptoCurrency _ripple;

        public decimal UsdSellRate { get; set; }

        public IEnumerable<Dto.CryptoCurrency> GetCurrentCryptoCurrency()
        {
            var apiClient = new ApiClient();

            var resp = apiClient.GetWebResponse("https://www.bitstamp.net/api/v2/ticker/xrpusd");

            var ticker = JsonConvert.DeserializeObject<JsonTicker>(resp.ResponseData);

            var now = DateTime.UtcNow;

            var list = new List<Dto.CryptoCurrency>();

            _ripple = new Dto.CryptoCurrency
            {
                UsdValue = ticker.last,
                Currency = Currency.Ripple,
                Provider = Provider.BitStamp,
                ModifiedAt = now
            };

            list.Add(_ripple);

            return list;
        }

    }
}
