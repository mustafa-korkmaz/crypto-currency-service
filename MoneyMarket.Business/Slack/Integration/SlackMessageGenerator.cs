
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoneyMarket.Business.Setting;
using MoneyMarket.Common;
using MoneyMarket.Common.Helper;

namespace MoneyMarket.Business.Slack.Integration
{
    public static class SlackMessageGenerator
    {
        public static string GetCryptoCurrencyBalanceMessage(IEnumerable<Dto.TeamCryptoCurrencyBalance> teamCryptoCurrencybalances, MainCurrency teamMainCurrency, IEnumerable<Dto.CryptoCurrency> cryptoCurrencies, string successText)
        {
            var retMessage = new StringBuilder();

            decimal usdSellRate = 0;

            if (teamMainCurrency == MainCurrency.Try)
            {
                var settingBusiness = new SettingBusiness();

                usdSellRate = settingBusiness.GetUsdValue();
            }

            decimal totalUsdValueOfCryptoCurrencies = 0;

            foreach (var tccb in teamCryptoCurrencybalances)
            {
                tccb.UnitUsdValue = cryptoCurrencies.First(p => p.Currency == tccb.Currency).UsdValue;
                var balanceLine = string.Format("{0}: {1} {2:G}{3}", tccb.Name, tccb.Balance.ToMoneyMarketCryptoCurrencyFormat(), tccb.Currency, "{lf}");

                retMessage.Append(balanceLine);

                totalUsdValueOfCryptoCurrencies += tccb.TotalUsdValue;
            }

            if (teamMainCurrency == MainCurrency.Try)
            {
                var totalTryValueOfCryptoCurrencies = totalUsdValueOfCryptoCurrencies * usdSellRate;

                retMessage.Append(string.Format("{0} {1} {2:G} ({3} {4:G})", successText, totalUsdValueOfCryptoCurrencies.ToMoneyMarketMoneyFormat(), MainCurrency.Usd, totalTryValueOfCryptoCurrencies.ToMoneyMarketMoneyFormat(), MainCurrency.Try));
            }
            else
                retMessage.Append(string.Format("{0} {1} {2:G}", successText, totalUsdValueOfCryptoCurrencies.ToMoneyMarketMoneyFormat(), teamMainCurrency));


            return retMessage.ToString();
        }

        public static string GetCryptoCurrencyMessage(IEnumerable<Dto.CryptoCurrency> cryptoCurrencies, MainCurrency teamMainCurrency)
        {
            var retMessage = new StringBuilder();

            decimal usdSellRate = 0;

            if (teamMainCurrency == MainCurrency.Try)
            {
                var settingBusiness = new SettingBusiness();

                usdSellRate = settingBusiness.GetUsdValue();
            }

            foreach (var cryptoCurrency in cryptoCurrencies)
            {
                var usdValueStr = cryptoCurrency.UsdValue.ToMoneyMarketCryptoCurrencyFormat();

                var tryValueStr = teamMainCurrency == MainCurrency.Usd ? "" : string.Format(" ({0} {1:G})", (cryptoCurrency.UsdValue * usdSellRate).ToMoneyMarketMoneyFormat(), MainCurrency.Try);

                if (teamMainCurrency == MainCurrency.Try)
                {
                    retMessage.Append(string.Format("{0:G} {1:G}: {2} {3:G}{4}{5}", cryptoCurrency.Provider, cryptoCurrency.Currency, usdValueStr, MainCurrency.Usd, tryValueStr, "{lf}"));
                }
                else
                    retMessage.Append(string.Format("{0:G} {1:G}: {2} {3:G}{4}", cryptoCurrency.Provider, cryptoCurrency.Currency, usdValueStr, teamMainCurrency, "{lf}"));

            }

            return retMessage.ToString();
        }

        public static string GetCryptoCurrencyAlarmMessage(IEnumerable<Dto.CryptoCurrency> cryptoCurrencies, MainCurrency mainCurrency, decimal usdSellRate)
        {
            var retMessage = new StringBuilder();

            foreach (var cryptoCurrency in cryptoCurrencies)
            {
                var usdValueStr = cryptoCurrency.UsdValue.ToMoneyMarketCryptoCurrencyFormat();

                var tryValueStr = mainCurrency == MainCurrency.Usd ? "" : string.Format(" ({0} {1:G})", (cryptoCurrency.UsdValue * usdSellRate).ToMoneyMarketMoneyFormat(), MainCurrency.Try);

                var alarmEmoji = ":bell: ";

                var cryptoCurrencyLine = string.Format("{0}{1:G} {2:G}: {3} {4:G}{5}{6}", alarmEmoji, cryptoCurrency.Provider, cryptoCurrency.Currency, usdValueStr, mainCurrency, tryValueStr, "{lf}");

                retMessage.Append(cryptoCurrencyLine);
            }

            return retMessage.ToString();
        }

        public static string GetAlarmMessage(string message, Currency currency, decimal limitAmount, MainCurrency teamMainCurrency)
        {
            var valStr = limitAmount.ToMoneyMarketCryptoCurrencyFormat();

            return string.Format(message, currency, valStr, teamMainCurrency);
        }

        public static string GetInvestmentMessage(IEnumerable<Dto.TeamInvestment> teamInvestments, MainCurrency teamMainCurrency, string successText)
        {
            var retMessage = new StringBuilder();

            decimal totalValueOfInvestments = 0;

            foreach (var investment in teamInvestments)
            {
                var invAmount = 0 - investment.Balance;
                var balanceLine = string.Format("{0} {1:G}: {2} {3:G}{4}", investment.Name, investment.Currency, invAmount.ToMoneyMarketMoneyFormat(), teamMainCurrency, "{lf}");

                retMessage.Append(balanceLine);

                totalValueOfInvestments += invAmount;
            }

            retMessage.Append(string.Format("{0} {1} {2:G}", successText, totalValueOfInvestments.ToMoneyMarketMoneyFormat(), teamMainCurrency));

            return retMessage.ToString();
        }

        public static string GetRevenueMessage(IEnumerable<Dto.TeamInvestment> teamRevenues, MainCurrency teamMainCurrency, string successText)
        {
            var retMessage = new StringBuilder();

            decimal totalValueOfRevenues = 0;

            foreach (var revenue in teamRevenues)
            {
                var balanceLine = string.Format("{0} {1:G}: {2} {3:G}{4}", revenue.Name, revenue.Currency, revenue.Balance.ToMoneyMarketMoneyFormat(), teamMainCurrency, "{lf}");

                retMessage.Append(balanceLine);

                totalValueOfRevenues += revenue.Balance;
            }

            retMessage.Append(string.Format("{0} {1} {2:G}", successText, totalValueOfRevenues.ToMoneyMarketMoneyFormat(), teamMainCurrency));

            return retMessage.ToString();
        }

        public static string GetArbitrageMessage(IEnumerable<Dto.CryptoCurrency> cryptoCurrencies, Currency currency)
        {
            var filteredCryptoCurrencies = cryptoCurrencies
                .Where(p => p.Currency == Currency.Btc || p.Currency == Currency.Eth)
                .OrderBy(p => p.Currency).ThenBy(p => p.UsdValue);

            var ethMin = filteredCryptoCurrencies.FirstOrDefault(p => p.Currency == Currency.Eth);
            var ethMax = filteredCryptoCurrencies.LastOrDefault(p => p.Currency == Currency.Eth);
            decimal ethProfitPercentage = 0;
            string ethText = string.Empty;

            if (ethMin != null && ethMax != null)
            {
                var ethDiff = ethMax.UsdValue - ethMin.UsdValue;
                ethProfitPercentage = 100 * ethDiff / ethMax.UsdValue;

                ethText =
                    $"{ethMax.Provider:G}/{ethMin.Provider:G} Eth Diff\n{ethMax.UsdValue.ToMoneyMarketMoneyFormat()} - {ethMin.UsdValue.ToMoneyMarketMoneyFormat()} = {ethDiff.ToMoneyMarketMoneyFormat()} Usd\n";
            }

            var btcMin = filteredCryptoCurrencies.FirstOrDefault(p => p.Currency == Currency.Btc);
            var btcMax = filteredCryptoCurrencies.LastOrDefault(p => p.Currency == Currency.Btc);

            decimal btcProfitPercentage = 0;
            string btcText = string.Empty;

            if (btcMin != null && btcMax != null)
            {
                var btcDiff = btcMax.UsdValue - btcMin.UsdValue;
                btcProfitPercentage = 100 * btcDiff / btcMax.UsdValue;
                btcText =
                    $"{btcMax.Provider:G}/{btcMin.Provider:G} Btc Diff\n{btcMax.UsdValue.ToMoneyMarketMoneyFormat()} - {btcMin.UsdValue.ToMoneyMarketMoneyFormat()} = {btcDiff.ToMoneyMarketMoneyFormat()} Usd\n";
            }

            var ethProfitText = Math.Abs(ethProfitPercentage) > 0 ? $"Eth profit = {ethProfitPercentage.ToMoneyMarketMoneyFormat()}\n" : "";
            var btcProfitText = Math.Abs(btcProfitPercentage) > 0 ? $"Btc profit = {btcProfitPercentage.ToMoneyMarketMoneyFormat()}\n" : "";

            //return all
            var message = $"{ethText}{btcText}{ethProfitText}{btcProfitText}";
            return message;
        }

        public static string GetArbitrageAlarmMessage(IEnumerable<Dto.CryptoCurrency> cryptoCurrencies, Currency currency, decimal profitLimitAmount)
        {
            var filteredCryptoCurrencies = cryptoCurrencies
                .Where(p => p.Currency == Currency.Btc || p.Currency == Currency.Eth)
                .OrderBy(p => p.Currency).ThenBy(p => p.UsdValue);

            var ethMin = filteredCryptoCurrencies.FirstOrDefault(p => p.Currency == Currency.Eth);
            var ethMax = filteredCryptoCurrencies.LastOrDefault(p => p.Currency == Currency.Eth);
            decimal ethProfitPercentage = 0;
            string ethText = string.Empty;

            if (ethMin != null && ethMax != null)
            {
                var ethDiff = ethMax.UsdValue - ethMin.UsdValue;
                ethProfitPercentage = 100 * ethDiff / ethMax.UsdValue;

                ethText =
                    $"{ethMax.Provider:G}/{ethMin.Provider:G} Eth Diff\n{ethMax.UsdValue.ToMoneyMarketMoneyFormat()} - {ethMin.UsdValue.ToMoneyMarketMoneyFormat()} = {ethDiff.ToMoneyMarketMoneyFormat()} Usd\n";
            }

            var btcMin = filteredCryptoCurrencies.FirstOrDefault(p => p.Currency == Currency.Btc);
            var btcMax = filteredCryptoCurrencies.LastOrDefault(p => p.Currency == Currency.Btc);

            decimal btcProfitPercentage = 0;
            string btcText = string.Empty;

            if (btcMin != null && btcMax != null)
            {
                var btcDiff = btcMax.UsdValue - btcMin.UsdValue;
                btcProfitPercentage = 100 * btcDiff / btcMax.UsdValue;
                btcText =
                    $"{btcMax.Provider:G}/{btcMin.Provider:G} Btc Diff\n{btcMax.UsdValue.ToMoneyMarketMoneyFormat()} - {btcMin.UsdValue.ToMoneyMarketMoneyFormat()} = {btcDiff.ToMoneyMarketMoneyFormat()} Usd\n";
            }

            var alarmEmoji = ":bell:";
            var ethMessage = Math.Abs(ethProfitPercentage) > profitLimitAmount ? $"{alarmEmoji}{ethText}Eth profit = {ethProfitPercentage.ToMoneyMarketMoneyFormat()}\n" : "";
            var btcMessage = Math.Abs(btcProfitPercentage) > profitLimitAmount ? $"{alarmEmoji}{btcText}Btc profit = {btcProfitPercentage.ToMoneyMarketMoneyFormat()}\n" : "";

            //return one of btc or eth

            if (currency == Currency.Btc)
            {
                return btcMessage;
            }

            return ethMessage;
        }
    }
}