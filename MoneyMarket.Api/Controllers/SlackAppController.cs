using System.Web.Http;
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
        /// <summary>
        /// gets slack event subscriptions
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("event/subscribe")]
        public IHttpActionResult Subscribe([FromBody]SlackAppRequest request)
        {
            var model = new SlackAppViewModel
            {
                Challenge = request.Challenge
            };

            return Ok(model); //responses to slack that we are good.
        }
    }
}