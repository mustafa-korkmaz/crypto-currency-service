using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MoneyMarket.Business.CryptoCurrency;
using MoneyMarket.Common;
using MoneyMarket.Common.ApiObjects.Response.ViewModels;
using MoneyMarket.Common.Helper;
using MoneyMarket.Common.Response;

namespace MoneyMarket.Api.Controllers
{
    /// <summary>
    /// money controller
    /// </summary>
    [RoutePrefix("CryptoCurrency")]
    public class CryptoCurrencyController : ApiBaseController
    {
        private readonly CryptoCurrencyBusiness _cryptoCurrencyBusiness = new CryptoCurrencyBusiness();

        // Get
        /// <summary>
        /// lists all moneys
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ticker")]
        public IHttpActionResult List()
        {
            var cryptoCurrencyList = GetAllCryptoCurrencies();
            return Ok(cryptoCurrencyList);
        }

        // Get/Refresh
        /// <summary>
        /// refreshes all moneys
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("refresh")]
        public IHttpActionResult Refresh()
        {
            var resp = RefreshTickers();

            if (resp.ResponseCode != ResponseCode.Success)
            {
                return BadRequest(resp.ResponseMessage);
            }

            return Ok();
        }

        // Get
        /// <summary>
        /// returns current crypto currencies data 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("arbitrage")]
        public IHttpActionResult Arbitrage()
        {
            var arbitrages = GetArbitrages();
            return Ok(arbitrages);
        }

        private ArbitrageViewModel GetArbitrages()
        {
            var ticker = _cryptoCurrencyBusiness.GetTicker();

            var tickerViewModel = new ArbitrageViewModel
            {
                UsdSellRate = ticker.UsdSellRate.ToDropMoneyFormat()
            };

            tickerViewModel.Arbitrages = new List<Arbitrage>
            {
                new Arbitrage
                {
                    Name = "BTCTurk-BitStamp",
                    Currency = "ETH",
                    Diff = (ticker.BtcTurkEth.UsdValue - ticker.BitStampEth.UsdValue).ToDropMoneyFormat(),
                },
                new Arbitrage
                {
                    Name = "BTCTurk-BitStamp",
                    Currency = "BTC",
                    Diff = (ticker.BtcTurkBtc.UsdValue - ticker.BitStampBtc.UsdValue).ToDropMoneyFormat(),
                }
            };

            return tickerViewModel;
        }

        private BusinessResponse RefreshTickers()
        {
            return _cryptoCurrencyBusiness.RefreshTickers();
        }

        private IEnumerable<CryptoCurrencyViewModel> GetAllCryptoCurrencies()
        {
            var cryptoCurrencies = _cryptoCurrencyBusiness.All();

            var cryptoCurrencyViewModels = cryptoCurrencies.Select(p => new CryptoCurrencyViewModel
            {
                Id = p.Id,
                Provider = p.Provider.ToString("G"),
                Currency = p.Currency.ToString("G"),
                UsdValue = p.UsdValue.ToDropMoneyFormat(),
                ModifiedAt = p.ModifiedAt.ToDropDateTimeInSecondsFormat()
            })
            .OrderBy(p => p.Currency)
            .ThenBy(p => p.Provider)
            .ToList();

            return cryptoCurrencyViewModels;
        }
    }
}