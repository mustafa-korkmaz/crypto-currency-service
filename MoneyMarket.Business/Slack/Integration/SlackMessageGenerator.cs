
using System.Collections.Generic;
using System.Text;

namespace MoneyMarket.Business.Slack.Integration
{
    public static class SlackMessageGenerator
    {
        public static string GetCryptoCurrencyBalanceMessage(IEnumerable<Dto.TeamCryptoCurrencyBalance> teamCryptoCurrencybalances)
        {
            var retMessage = new StringBuilder();

            foreach (var tccb in teamCryptoCurrencybalances)
            {
                var balanceLine = string.Format("{0}: {1} {2}{3}", tccb.Name, tccb.Balance, tccb.Currency.ToString("G"), "{lf}");
                retMessage.Append(balanceLine);
            }

            return retMessage.ToString();
        }
    }
}
