using MoneyMarket.Business.HttpClient;
using MoneyMarket.Business.Slack.Integration;
using MoneyMarket.Common;
using MoneyMarket.Common.Helper;
using MoneyMarket.DataAccess;

namespace MoneyMarket.Business.Notification
{
    public class NotificationBusiness
    {
        private readonly UnitOfWork _uow = new UnitOfWork();
        private readonly IRepository<DataAccess.Models.CryptoCurrency> _repository;

        public NotificationBusiness()
        {
            _repository = _uow.Repository<DataAccess.Models.CryptoCurrency>();
        }

        public void SendXlmNotification()
        {
            var xlm = _repository.GetById(DatabaseKey.CryptoCurrency.Xlm);

            SendSlackNotification(xlm.UsdValue);
        }

        private void SendSlackNotification(decimal usdValue)
        {
            var slackToken = Statics.GetConfigKey(ConfigKeys.SlackTokenAkhisar);
            var xlmBalance = decimal.Parse(Statics.GetConfigKey(ConfigKeys.XlmBalance));

            var usdTotal = xlmBalance * usdValue;

            var payload = new SlackMessage
            {
                token = slackToken,
                channel = "#xlm",
                text = $"Bakiye= {xlmBalance} XLM ({usdTotal.ToDropMoneyFormat()} USD)"
            };

            var slackClient = SlackApiClient.Instance;

            slackClient.InvokeApi<SlackMessage, object>("chat.postMessage", payload);
        }

    }
}
