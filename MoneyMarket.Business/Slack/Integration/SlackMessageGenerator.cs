
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
                var settingBusiness = new SettingBusiness();

                totalUsdValueOfCryptoCurrencies *= settingBusiness.GetUsdValue();
            }

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
                var valueStr = usdSellRate == 0 ? cryptoCurrency.UsdValue.ToMoneyMarketCryptoCurrencyFormat() : (cryptoCurrency.UsdValue * usdSellRate).ToMoneyMarketMoneyFormat();

                var cryptoCurrencyLine = string.Format("{0:G} {1:G}: {2} {3:G}{4}", cryptoCurrency.Provider, cryptoCurrency.Currency, valueStr, teamMainCurrency, "{lf}");

                retMessage.Append(cryptoCurrencyLine);
            }

            return retMessage.ToString();
        }

        public static string GetCryptoCurrencyAlarmMessage(IEnumerable<Dto.CryptoCurrency> cryptoCurrencies, MainCurrency mainCurrency, decimal usdSellRate)
        {
            var retMessage = new StringBuilder();

            foreach (var cryptoCurrency in cryptoCurrencies)
            {
                var valueStr = mainCurrency == MainCurrency.Usd ? cryptoCurrency.UsdValue.ToMoneyMarketCryptoCurrencyFormat() : (cryptoCurrency.UsdValue * usdSellRate).ToMoneyMarketMoneyFormat();

                var alarmEmoji = ":bell: ";

                var cryptoCurrencyLine = string.Format("{0}{1:G} {2:G}: {3} {4:G}{5}", alarmEmoji, cryptoCurrency.Provider, cryptoCurrency.Currency, valueStr, mainCurrency, "{lf}");

                retMessage.Append(cryptoCurrencyLine);
            }

            return retMessage.ToString();
        }


        public static string GetAlarmMessage(string message, Currency currency, decimal limitAmount, MainCurrency teamMainCurrency)
        {
            var valStr = limitAmount.ToMoneyMarketCryptoCurrencyFormat();

            return string.Format(message, currency, valStr, teamMainCurrency);
        }
    }
}
