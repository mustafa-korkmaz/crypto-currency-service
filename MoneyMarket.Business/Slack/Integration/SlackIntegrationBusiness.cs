using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using MoneyMarket.Common;
using MoneyMarket.Common.ApiObjects.Request.SlackApp;
using MoneyMarket.Common.ApiObjects.Response.SlackApp;
using MoneyMarket.Common.Helper;
using MoneyMarket.Common.Response;
using MoneyMarket.Dto;
using MoneyMarket.Business.CryptoCurrency;

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
                this.Parameters = GetExecutionCommandParameters(eventSubscriptionRequest.Event.Text);

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
            await PostMessage(GetSlackExecutionSuccessMessage());
        }

        /// <summary>
        /// posts command list to channel.
        /// scope= chat:general
        /// cmd= 'help'
        /// </summary>
        /// <returns></returns>
        public override async Task Help()
        {
            await PostMessage(GetSlackExecutionSuccessMessage());
        }

        /// <summary>
        /// scope= set:settings
        /// cmd= 'set lang @p0'
        /// </summary>
        /// <returns></returns>
        public override async Task SetLanguage()
        {
            int parameterCount = 1;
            var parameterSet = new List<CommandParameter>
            {
                new CommandParameter
                {
                    Depth = 2,
                    ParameterValue = Parameters[0].ToLower(),
                    ParameterSet = new List<string>
                    {
                        "en",
                        "tr"
                    }
                }
            };

            var validateResp = ValidateParameters(parameterSet, parameterCount);

            if (validateResp.ResponseCode != ResponseCode.Success)
            {
                await PostMessage(GetSlackExecutionErrorMessage(validateResp.ResponseData));
                return;
            }

            var teamBusiness = new TeamBusiness();

            var lang = Statics.GetLanguage(Parameters[0]);

            if (lang == Language.Unknown)
            {
                await PostMessage(GetSlackExecutionErrorMessage(3));
                return;
            }


            SetTeamLanguage(lang);

            teamBusiness.Edit(Team);
            await PostMessage(GetSlackExecutionSuccessMessage());
        }

        /// <summary>
        /// scope= set:settings
        /// cmd= 'set currency @p0'
        /// @p0 parameter for desired currency
        /// </summary>
        /// <returns></returns>
        public override async Task SetCurrency()
        {
            int parameterCount = 1;
            var parameterSet = new List<CommandParameter>
            {
                new CommandParameter
                {
                    Depth = 2,
                    ParameterValue = Parameters[0].ToLower(),
                    ParameterSet = new List<string>
                    {
                        "tl",
                        "usd"
                    }
                }
            };

            var validateResp = ValidateParameters(parameterSet, parameterCount);

            if (validateResp.ResponseCode != ResponseCode.Success)
            {
                await PostMessage(GetSlackExecutionErrorMessage(validateResp.ResponseData));
                return;
            }

            var teamBusiness = new TeamBusiness();

            var currency = Statics.GetMainCurrency(Parameters[0]);

            if (currency == MainCurrency.Unknown)
            {
                await PostMessage(GetSlackExecutionErrorMessage(3));
                return;
            }

            Team.MainCurrency = currency;

            teamBusiness.Edit(Team);
            await PostMessage(GetSlackExecutionSuccessMessage());
        }

        /// <summary>
        /// scope= set:balance
        /// cmd= 'set balance @p0 @p1 @p2'
        /// @p0 parameter for desired currency
        /// @p1 parameter for balance name
        /// @p2 parameter for balance amount
        /// </summary>
        /// <returns></returns>
        public override async Task SetBalance()
        {
            int parameterCount = 3;

            var validateResp = ValidateParameters(null, parameterCount);

            if (validateResp.ResponseCode != ResponseCode.Success)
            {
                await PostMessage(GetSlackExecutionErrorMessage(validateResp.ResponseData));
                return;
            }

            var currency = Statics.GetCurrency(Parameters[0]);

            if (currency == Currency.Unknown)
            {
                //post depth=2 message => Given crypto currency either not found or not supported.
                await PostMessage(GetSlackExecutionErrorMessage(2));
                return;
            }

            decimal balanceAmount = 0;

            if (!decimal.TryParse(Parameters[2], out balanceAmount))
            {
                //post depth=3 message => Balance amount is invalid. Use only . (dot) and numbers for balances.
                await PostMessage(GetSlackExecutionErrorMessage(3));
                return;
            }

            //everytihng is fine. add or update balance.
            var teamCryptoCurrencyBalanceBusiness = new TeamCryptoCurrencyBalanceBusiness();

            var balance = new TeamCryptoCurrencyBalance
            {
                TeamId = Team.Id,
                Currency = currency,
                Name = Parameters[1].ToLower(),
                Balance = decimal.Parse(Parameters[2])
            };

            teamCryptoCurrencyBalanceBusiness.Add(balance);

            await PostMessage(GetSlackExecutionSuccessMessage());
        }

        /// <summary>
        /// scope= get:balance
        /// cmd= 'get balance @p0'
        /// @p0 parameter for desired currency (all for all balances)
        /// </summary>
        /// <returns></returns>
        public override async Task GetBalance()
        {
            int parameterCount = 1;

            var validateResp = ValidateParameters(null, parameterCount);

            if (validateResp.ResponseCode != ResponseCode.Success)
            {
                await PostMessage(GetSlackExecutionErrorMessage(validateResp.ResponseData));
                return;
            }

            var currency = Currency.Unknown;
            var parameter = Parameters[0];

            if (parameter.ToLower() != "all")
            {
                currency = Statics.GetCurrency(Parameters[0]);

                if (currency == Currency.Unknown)
                {
                    //post depth=2 message => Given crypto currency either not found or not supported.
                    await PostMessage(GetSlackExecutionErrorMessage(2));
                    return;
                }
            }

            var balances = GetTeamCryptoCurrencyBalances(currency);

            var successMessage = SlackMessageGenerator.GetCryptoCurrencyBalanceMessage(balances);
            await PostMessage(GetSlackExecutionSuccessMessage(successMessage));
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

                method.Invoke(this, null);
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


        private IEnumerable<Dto.TeamCryptoCurrencyBalance> GetTeamCryptoCurrencyBalances(Currency currency)
        {
            var teamCryptoCurrencyBalanceBusiness = new TeamCryptoCurrencyBalanceBusiness();

            return teamCryptoCurrencyBalanceBusiness.GetTeamCryptoCurrencyBalances(Team.Id, currency);
        }

    }
}
