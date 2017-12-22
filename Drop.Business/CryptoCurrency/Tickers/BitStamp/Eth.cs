using System.Collections.Generic;
using MoneyMarket.Business.HttpClient;
using MoneyMarket.Common;
using MoneyMarket.Common.Helper;
using Newtonsoft.Json;

namespace MoneyMarket.Business.CryptoCurrency.Tickers.BitStamp
{
    public class Eth : ITicker
    {
        private Dto.CryptoCurrency _eth;

        public decimal UsdSellRate { get; set; }

        public IEnumerable<Dto.CryptoCurrency> GetCurrentCryptoCurrency()
        {
            var apiClient = new ApiClient();

            var resp = apiClient.GetWebResponse("https://www.bitstamp.net/api/v2/ticker/ethusd");


            var ticker = JsonConvert.DeserializeObject<JsonTicker>(resp.ResponseData);

            var now = Statics.GetTurkeyCurrentDateTime();

            var list = new List<Dto.CryptoCurrency>();

            _eth = new Dto.CryptoCurrency
            {
                UsdValue = ticker.last,
                Currency = Currency.Eth,
                Provider = Provider.BitStamp,
                ModifiedAt = now
            };

            list.Add(_eth);

            return list;
        }

    }
}
