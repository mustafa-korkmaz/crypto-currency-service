using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyMarket.Common.ApiObjects.Request.SlackApp;
using MoneyMarket.Common.ApiObjects.Response.SlackApp;

namespace MoneyMarket.Business.Slack.Integration
{
    interface ISlackIntegration
    {
        /// <summary>
        /// application authorization
        /// </summary>
        Task<OAuthResponse> OAuthAccess(string accessCode);

        /// <summary>
        /// events api subscription handler
        /// </summary>
        /// <param name="eventSubscriptionRequest"></param>
        void SubscribeEvent(SlackEventSubscriptionRequest eventSubscriptionRequest);

        /// <summary>
        /// bot message writer
        /// </summary>
        /// <param name="message"></param>
        Task<SlackResponse> PostMessage(SlackMessage message);

        /// <summary>
        /// returns user list joined to team
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        IEnumerable<SlackUser> GetUserList(Dto.Team team);
    }
}
