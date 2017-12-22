using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyMarket.Business.HttpClient;

namespace MoneyMarket.UnitTests
{
    [TestClass]
    public class SlackApiTest
    {
        [TestMethod]
        public void PostMessageViaBot()
        {
            var payload = new SlackMessage
            {
                token = "xoxb-280481595985-B9HRMZDOd5NHWUHBWqrUbXFP",
                channel = "#general",
                text = "deneme 1-2-3"
            };

            var slackClient = SlackApiClient.Instance;

            var resp = slackClient.InvokeApi<SlackMessage, object>("chat.postMessage", payload);
        }


       
    }
}
