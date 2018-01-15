
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
                var balanceLine = string.Format("{0} {1:G}: {2} {3:G}{4}", investment.Name, investment.Currency, investment.Balance.ToMoneyMarketMoneyFormat(), teamMainCurrency, "{lf}");

                retMessage.Append(balanceLine);

                totalValueOfInvestments += investment.Balance;
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

    }
}
