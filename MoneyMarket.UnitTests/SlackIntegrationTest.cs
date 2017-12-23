using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyMarket.Business.Slack.Integration;
using MoneyMarket.Common;

namespace MoneyMarket.UnitTests
{
    [TestClass]
    public class SlackIntegrationTest
    {
        [TestMethod]
        public async void PostMessageViaBot()
        {
            var payload = new SlackMessage
            {
                channel = "#general",
                token = "xoxb-290876090100-HByhoYtz4LNcADDfFZthAO0v",
                text = SlackBotMessage.Welcome
            };
            var slackIntegrationBusiness = new SlackIntegrationBusiness();

            //say hello to new team!
            var resp = await slackIntegrationBusiness.PostMessage(payload);

            Assert.IsTrue(resp.ok);
        }

    }
}
