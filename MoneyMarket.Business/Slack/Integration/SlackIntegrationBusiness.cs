using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using MoneyMarket.Common;
using MoneyMarket.Common.ApiObjects.Request.SlackApp;
using MoneyMarket.Common.ApiObjects.Response.SlackApp;
using MoneyMarket.Dto;

namespace MoneyMarket.Business.Slack.Integration
{
    public class SlackIntegrationBusiness : ISlackIntegration
    {
        private SlackApiClient _client = SlackApiClient.Instance;

        public async Task<OAuthResponse> OAuthAccess(string accessCode)
        {
            AppSettingsReader configurationAppSettings = new AppSettingsReader();

            var clientId = (string)configurationAppSettings.GetValue(ConfigKeys.SlackClientId, typeof(string));

            var clientSecret = (string)configurationAppSettings.GetValue(ConfigKeys.SlackClientSecret, typeof(string));

            var request = new OAuthRequest
            {
                client_secret = clientSecret,
                client_id = clientId,
                code = accessCode,
                redirect_uri = ApiUrl.SlackRedirectUri
            };

            var resp = await _client.InvokeApi<OAuthRequest, OAuthResponse>(ApiUrl.SlackOAuth, request);

            return resp.ResponseData;
        }

        public void SubscribeEvent(SlackEventSubscriptionRequest eventSubscriptionRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<SlackResponse> PostMessage(SlackMessage message)
        {
            var resp = await _client.InvokeApi<SlackMessage, SlackResponse>("chat.postMessage", message);

            return resp.ResponseData;
        }

        public IEnumerable<SlackUser> GetUserList(Team team)
        {
            throw new NotImplementedException();
        }
    }
}
