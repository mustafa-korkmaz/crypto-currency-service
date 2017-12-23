﻿using System;
using System.Collections.Generic;
using System.Globalization;
using MoneyMarket.Common;
using MoneyMarket.Common.Response;
using MoneyMarket.DataAccess;
using MoneyMarket.Dto;
using System.Linq;
using MoneyMarket.Business.CryptoCurrency.Tickers;
using MoneyMarket.Business.HttpClient;
using MoneyMarket.Business.Slack.Integration;
using MoneyMarket.Common.Helper;
using Newtonsoft.Json;

namespace MoneyMarket.Business.CryptoCurrency
{
    public class CryptoCurrencyBusiness
    {
        private readonly UnitOfWork _uow = new UnitOfWork();
        private readonly IRepository<DataAccess.Models.CryptoCurrency> _repository;
        private const string CryptoCurrencyAssemblyInfo = "MoneyMarket.Business.CryptoCurrency.Tickers";

        public CryptoCurrencyBusiness()
        {
            _repository = _uow.Repository<DataAccess.Models.CryptoCurrency>();
        }

        public Ticker GetTicker()
        {
            var settingRepository = _uow.Repository<DataAccess.Models.Setting>();

            var usdSetting = settingRepository.GetById(DatabaseKey.Setting.UsdSellRate);

            var ticker = new Ticker
            {
                UsdSellRate = Convert.ToDecimal(usdSetting.Value)
            };

            var allCurrencies = All(); //get latest records from db.

            ticker.BitStampBtc =
                allCurrencies.First(p => p.Currency == Currency.Btc && p.Provider == Provider.BitStamp);

            ticker.BitStampEth =
                allCurrencies.First(p => p.Currency == Currency.Eth && p.Provider == Provider.BitStamp);

            ticker.BtcTurkBtc =
                allCurrencies.First(p => p.Currency == Currency.Btc && p.Provider == Provider.BtcTurk);

            ticker.BtcTurkEth =
                allCurrencies.First(p => p.Currency == Currency.Eth && p.Provider == Provider.BtcTurk);

            return ticker;
        }

        public BusinessResponse RefreshTickers()
        {
            var resp = new BusinessResponse
            {
                ResponseCode = ResponseCode.Fail
            };

            var usdSellRate = GetLatestUsdSellRate();

            var cryptoCurrencies = new List<Dto.CryptoCurrency>();

            var refresherClasses = GetRefresherClassNames();


            foreach (var refresherClass in refresherClasses)
            {
                var cryptoCurrencyClassName = GetCryptoCurrencyClassName(refresherClass);
                var type = Type.GetType(cryptoCurrencyClassName);

                var obj = Activator.CreateInstance(type, true);

                var ticker = obj as ITicker;

                ticker.UsdSellRate = usdSellRate;

                cryptoCurrencies.AddRange(ticker.GetCurrentCryptoCurrency());
            }

            SendSlackNotification(cryptoCurrencies);

            UpdateCryptoCurrencies(cryptoCurrencies);
            UpdateUsdSellRate(usdSellRate);

            resp.ResponseCode = ResponseCode.Success;
            return resp;
        }

        public IEnumerable<Dto.CryptoCurrency> All()
        {
            var webSites = _repository.AsQueryable()
                .Select(p => new Dto.CryptoCurrency
                {
                    Id = p.Id,
                    Provider = p.Provider,
                    Currency = p.Currency,
                    ClassName = p.ClassName,
                    UsdValue = p.UsdValue,
                    ModifiedAt = p.ModifiedAt
                })
                .ToList();

            return webSites;
        }

        private void UpdateCryptoCurrencies(IEnumerable<Dto.CryptoCurrency> cryptoCurrencies)
        {
            var allCurrencies = All(); //get latest records from db.

            foreach (var cryptoCurrency in cryptoCurrencies)
            {
                //match cryptoCurrency object with provider name and currency and set Id
                cryptoCurrency.Id = allCurrencies
                    .First(p => p.Provider == cryptoCurrency.Provider && p.Currency == cryptoCurrency.Currency).Id;
                Edit(cryptoCurrency);
            }
        }

        private void UpdateUsdSellRate(decimal latestUsdSellRate)
        {
            var settingRepository = _uow.Repository<DataAccess.Models.Setting>();

            var usdSetting = settingRepository.GetById(DatabaseKey.Setting.UsdSellRate);

            usdSetting.Value = latestUsdSellRate.ToString(CultureInfo.InvariantCulture);

            settingRepository.Update(usdSetting);

            _uow.Save();
        }

        private void Edit(Dto.CryptoCurrency dto)
        {
            var entity = _repository.GetById(dto.Id);

            entity.UsdValue = dto.UsdValue;
            entity.ModifiedAt = dto.ModifiedAt;

            _repository.Update(entity);

            _uow.Save();
        }

        private decimal GetLatestUsdSellRate()
        {
            var apiClient = new ApiClient();

            var resp = apiClient.GetWebResponse("https://www.doviz.com/api/v1/currencies/USD/latest");

            var usd = JsonConvert.DeserializeObject<JsonCurrency>(resp.ResponseData);

            return usd.selling;

        }

        private void SendSlackNotification(IEnumerable<Dto.CryptoCurrency> cryptoCurrencies)
        {
            var bitStampEth = cryptoCurrencies.First(p => p.Currency == Currency.Eth && p.Provider == Provider.BitStamp);

            var btcTurkEth = cryptoCurrencies.First(p => p.Currency == Currency.Eth && p.Provider == Provider.BtcTurk);

            var bitStampBtc = cryptoCurrencies.First(p => p.Currency == Currency.Btc && p.Provider == Provider.BitStamp);

            var btcTurkBtc = cryptoCurrencies.First(p => p.Currency == Currency.Btc && p.Provider == Provider.BtcTurk);

            var ethDiffLimit = decimal.Parse(Statics.GetConfigKey(ConfigKeys.EthDiff));

            var diffEth = btcTurkEth.UsdValue - bitStampEth.UsdValue;
            var ethProfitPercentage = GetProfitPercentage(btcTurkEth.UsdValue, diffEth);

            var diffBtc = btcTurkBtc.UsdValue - bitStampBtc.UsdValue;
            var btcProfitPercentage = GetProfitPercentage(btcTurkBtc.UsdValue, diffBtc);

            if (diffEth >= ethDiffLimit)
            {
                var slackToken = Statics.GetConfigKey(ConfigKeys.SlackToken);

                var ethText = $"BtcTurk/Bitstamp\nETH Diff\n{btcTurkEth.UsdValue.ToDropMoneyFormat()} - {bitStampEth.UsdValue.ToDropMoneyFormat()} = {diffEth.ToDropMoneyFormat()} USD";
                var btcText = $"BTC Diff\n{btcTurkBtc.UsdValue.ToDropMoneyFormat()} - {bitStampBtc.UsdValue.ToDropMoneyFormat()} = {diffBtc.ToDropMoneyFormat()} USD";
                var ethProfitText = $"ETH profit = {ethProfitPercentage.ToDropMoneyFormat()}";
                var btcProfitText = $"BTC profit = {btcProfitPercentage.ToDropMoneyFormat()}";
                var payload = new SlackMessage
                {
                    token = slackToken,
                    channel = "#general",
                    text = $"{ethText}\n{btcText}\n{ethProfitText}\n{btcProfitText}"
                };

                var slackClient = SlackApiClient.Instance;

                slackClient.InvokeApi<SlackMessage, object>("chat.postMessage", payload);
            }
        }

        private decimal GetProfitPercentage(decimal unitValue, decimal profit)
        {
            return 100 * profit / unitValue;
        }

        private string GetCryptoCurrencyClassName(string refresherClassName)
        {
            return $"{CryptoCurrencyAssemblyInfo}.{refresherClassName}";
        }

        private IEnumerable<string> GetRefresherClassNames()
        {
            var cryptoCurrencies = All();

            var refresherClasses = cryptoCurrencies.Select(p => $"{p.Provider:G}.{p.ClassName}");

            return refresherClasses.Distinct();
        }

    }
}