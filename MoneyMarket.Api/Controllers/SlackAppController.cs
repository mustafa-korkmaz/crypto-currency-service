using System.Web.Http;
using MoneyMarket.Business.Slack.Integration;
using MoneyMarket.Common.ApiObjects.Request.SlackApp;
using MoneyMarket.Common.ApiObjects.Response.ViewModels;

namespace MoneyMarket.Api.Controllers
{
    /// <summary>
    /// SlackApp controller
    /// </summary>
    [RoutePrefix("slackapp")]
    public class SlackAppController : ApiBaseController
    {
        private readonly SlackIntegrationBusiness _slackIntegration = new SlackIntegrationBusiness();

        /// <summary>
        /// gets slack event subscriptions
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("event/subscribe")]
        public IHttpActionResult Subscribe([FromBody]SlackEventSubscriptionRequest request)
        {
            var model = new SlackAppViewModel
            {
                Challenge = request.Challenge
            };

            // async run business, return OK to slack immediately
            _slackIntegration.SubscribeEvent(request);

            return Ok(model); //responses to slack that we are good.
        }
    }
}