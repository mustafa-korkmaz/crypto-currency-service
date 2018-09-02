using System;
using System.Collections.Generic;
using System.Linq;
using MoneyMarket.Business.HttpClient;
using MoneyMarket.Common;
using MoneyMarket.Common.Helper;
using Newtonsoft.Json;

namespace MoneyMarket.Business.CryptoCurrency.Tickers.BtcTurk
{
    public class BtcTurk : ITicker
    {
        private Dto.CryptoCurrency _eth;
        private Dto.CryptoCurrency _btc;
        private Dto.CryptoCurrency _xrp;
        private Dto.CryptoCurrency _ltc;

        public decimal UsdSellRate { get; set; }

        public IEnumerable<Dto.CryptoCurrency> GetCurrentCryptoCurrency()
        {
            var apiClient = new ApiClient();

            var resp = apiClient.GetWebResponse("https://www.btcturk.com/api/ticker");


            var tickers = JsonConvert.DeserializeObject<IEnumerable<JsonTicker>>(resp.ResponseData);

            var ethTicker = tickers.First(t => t.pair == "ETHTRY");
            var btcTicker = tickers.First(t => t.pair == "BTCTRY");

            var ltcTicker = tickers.First(t => t.pair == "LTCTRY");
            var xrpTicker = tickers.First(t => t.pair == "XRPTRY");

            var now = DateTime.UtcNow;

            var list = new List<Dto.CryptoCurrency>();

            _eth = new Dto.CryptoCurrency
            {
                UsdValue = ethTicker.last / UsdSellRate,
                Currency = Currency.Eth,
                Provider = Provider.BtcTurk,
                ModifiedAt = now
            };

            _btc = new Dto.CryptoCurrency
            {
                UsdValue = btcTicker.last / UsdSellRate,
                Currency = Currency.Btc,
                Provider = Provider.BtcTurk,
                ModifiedAt = now
            };

            _xrp = new Dto.CryptoCurrency
            {
                UsdValue = xrpTicker.last / UsdSellRate,
                Currency = Currency.Ripple,
                Provider = Provider.BtcTurk,
                ModifiedAt = now
            };

            _ltc = new Dto.CryptoCurrency
            {
                UsdValue = ltcTicker.last / UsdSellRate,
                Currency = Currency.Ltc,
                Provider = Provider.BtcTurk,
                ModifiedAt = now
            };

            list.Add(_eth);
            list.Add(_btc);
            list.Add(_xrp);
            list.Add(_ltc);

            return list;
        }

    }
}
