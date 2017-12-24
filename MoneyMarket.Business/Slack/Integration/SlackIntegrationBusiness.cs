using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using MoneyMarket.Common;
using MoneyMarket.Common.ApiObjects.Request.SlackApp;
using MoneyMarket.Common.ApiObjects.Response.SlackApp;
using MoneyMarket.Common.Response;
using MoneyMarket.Dto;

namespace MoneyMarket.Business.Slack.Integration
{
    public class SlackIntegrationBusiness : SlackCommandExecuter, ISlackIntegration
    {
        private readonly SlackApiClient _client = SlackApiClient.Instance;

        #region ISlackIntegration implementation

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

        public async Task SubscribeEvent(SlackEventSubscriptionRequest eventSubscriptionRequest)
        {
            // first check this event will be executed or not.
            var isEventSubscriptionValid = IsEventSubscriptionValid(eventSubscriptionRequest);

            if (isEventSubscriptionValid)
            {
                // now we know that this event includes a command. 
                // first set team
                SetTeam(eventSubscriptionRequest.TeamId);

                // set all commands using cahce
                SetCommands();

                // set channel
                SetChannel(eventSubscriptionRequest.Event.Channel);

                // set command properties
                var cmd = GetCommand(eventSubscriptionRequest);

                //check if it is a valid command or not.
                var cmdValidationResp = IsCommandValid(ref cmd);

                if (cmdValidationResp.ResponseCode != ResponseCode.Success)
                {
                    // this command is not valid or may be command is valid but the it won't be executed due to another error. 
                    //this case usually happens when user not have the rights to execute this command.
                    await PostMessage(cmdValidationResp.ResponseData);
                }

                //todo: process subscription
            }
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

        #endregion ISlackIntegration implementation

        #region SlackCommandExecuter implementations

        protected override BusinessResponse<SlackResponse> SayHello()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// posts command list to channel.
        /// scope= chat:general
        /// cmd= 'help'
        /// </summary>
        /// <returns></returns>
        protected override BusinessResponse<SlackResponse> Help()
        {
            throw new NotImplementedException();
        }

        protected override BusinessResponse<SlackResponse> SetLanguage(string culture)
        {
            throw new NotImplementedException();
        }

        #endregion SlackCommandExecuter implementations

        /// <summary>
        /// filters unacceptable event subscriptions
        /// </summary>
        /// <param name="eventSubscriptionRequest"></param>
        /// <returns></returns>
        private bool IsEventSubscriptionValid(SlackEventSubscriptionRequest eventSubscriptionRequest)
        {
            if (eventSubscriptionRequest.Type == "url_verification")
            {
                //this is a slack challange request for url verification. no need to execute anything
                return false;
            }

            if (string.IsNullOrEmpty(eventSubscriptionRequest.TeamId))
            {
                //we can do nothing without team id
                return false;
            }

            if (eventSubscriptionRequest.Event == null)
            {
                return false;
            }

            if (eventSubscriptionRequest.Event.SubType == "bot_message"
                || eventSubscriptionRequest.Event.SubType == "channel_join")
            {
                return false;
            }

            if (eventSubscriptionRequest.Event.Type == "message"
                && !string.IsNullOrEmpty(eventSubscriptionRequest.Event.Text))
            {
                return true;
            }

            //for other scenarios
            return false;
        }

        private Dto.Command GetCommand(SlackEventSubscriptionRequest eventSubscriptionRequest)
        {
            return new Dto.Command
            {
                Text = eventSubscriptionRequest.Event.Text
            };
        }
    }
}
