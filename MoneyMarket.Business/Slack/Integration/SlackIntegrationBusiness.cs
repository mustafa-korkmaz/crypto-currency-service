﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
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
                    return;
                }

                //process subscription

                //set executing cmd 
                this.ExecutingCommand = cmd;

                //set cmd parameters
                this.Parameters = GetExecutionCommandParameters(cmd.Text);

                //invoke related method
                InvokeExecutingCommandAction(cmd.Action);
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

        public override async Task SayHello()
        {
            await PostMessage(GetSlackSuccessMessage());
        }

        /// <summary>
        /// posts command list to channel.
        /// scope= chat:general
        /// cmd= 'help'
        /// </summary>
        /// <returns></returns>
        public override async Task Help()
        {
            await PostMessage(GetSlackSuccessMessage());
        }

        protected override async Task SetLanguage()
        {
            //todo: get language from parameters.
            var teamBusiness = new TeamBusiness();

            Team.Language = Language.English;

            teamBusiness.Edit(Team);
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

            if (eventSubscriptionRequest.Event.Subtype == "bot_message"
                || eventSubscriptionRequest.Event.Subtype == "channel_join")
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

        /// <summary>
        /// calls command's action method by name
        /// </summary>
        /// <param name="methodName"></param>
        private void InvokeExecutingCommandAction(string methodName)
        {
            try
            {
                Type thisType = this.GetType();
                MethodInfo method = thisType.GetMethod(methodName);

                if (this.Parameters == null)
                {
                    method.Invoke(this, null);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string[] GetExecutionCommandParameters(string cmdText)
        {
            var array = cmdText
                //.ToLower()
                .Replace("  ", " ") // more than one space char is unaccaptable
                .Split(' '); //split by space char

            if (array.Length == 1)
            {
                //parameterless command
                return null;
            }

            var list = array.ToList();

            // remove first 2 word which are command texts, not parameter.
            list.RemoveAt(0);
            list.RemoveAt(0);

            return list.ToArray();
        }
    }
}
