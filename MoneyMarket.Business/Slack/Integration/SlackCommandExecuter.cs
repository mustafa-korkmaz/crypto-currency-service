using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyMarket.Business.Command;
using MoneyMarket.Common;
using MoneyMarket.Common.Response;

namespace MoneyMarket.Business.Slack.Integration
{
    public abstract class SlackCommandExecuter
    {
        private readonly CommandBusiness _cmdBusiness = new CommandBusiness();
        private readonly TeamBusiness _teamBusiness = new TeamBusiness();
        private IEnumerable<Dto.Command> _commands;

        private string _channel;

        /// <summary>
        /// slack team fulfilled by event subscription request
        /// </summary>
        private Dto.Team _team;
        protected Dto.Team Team => _team;

        /// <summary>
        /// cmd which we want to execute at the moment.
        /// </summary>
        protected Dto.Command ExecutingCommand { get; set; }

        /// <summary>
        /// executing cmd parameters
        /// </summary>
        protected string[] Parameters { get; set; }

        /// <summary>
        /// scope= chat:general
        /// cmd text= 'hi'
        /// </summary>
        /// <returns></returns>
        public abstract Task SayHello();

        /// <summary>
        /// posts command list to channel.
        /// scope= chat:general
        /// cmd text= 'help'
        /// </summary>
        /// <returns></returns>
        public abstract Task Help();

        /// <summary>
        /// scope= set:settings
        /// cmd= 'set lang @p0'.
        /// @p0 parameter for desired language
        /// </summary>
        /// <returns></returns>
        public abstract Task SetLanguage();

        /// <summary>
        /// scope= set:settings
        /// cmd= 'set channel @p0'.
        /// @p0 parameter for channel info
        /// </summary>
        /// <returns></returns>
        public abstract Task SetChannel();

        /// <summary>
        /// scope= set:settings
        /// cmd= 'set currency @p0'.
        /// @p0 parameter for desired currency
        /// </summary>
        /// <returns></returns>
        public abstract Task SetCurrency();

        /// <summary>
        /// scope= set:balance
        /// cmd= 'set balance @p0 @p1 @p2'.
        /// @p0 parameter for desired currency
        /// @p1 parameter for balance name
        /// @p2 parameter for balance amount
        /// </summary>
        /// <returns></returns>
        public abstract Task SetBalance();

        /// <summary>
        /// scope= get:balance
        /// cmd= 'get balance @p0'.
        /// @p0 parameter for desired currency (all for all balances)
        /// </summary>
        /// <returns></returns>
        public abstract Task GetBalance();

        /// <summary>
        /// scope= set:alarm
        /// cmd= 'set balance @p0 @p1 @p2'.
        /// @p0 parameter for desired currency
        /// @p1 parameter for notification time interval in minutes
        /// </summary>
        /// <returns></returns>
        public abstract Task SetNotification();

        /// <summary>
        /// scope= set:alarms
        /// cmd= 'set balance @p0 @p1'.
        /// @p0 parameter for desired currency
        /// @p1 parameter for balance amount
        /// </summary>
        /// <returns></returns>
        public abstract Task SetAlarm();

        /// <summary>
        /// scope= list:currency
        /// cmd= 'get @p0.
        /// @p0 parameter for desired currency
        /// </summary>
        /// <returns></returns>
        public abstract Task GetCryptoCurrency();

        /// <summary>
        /// checks existance and authorizes command.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected virtual BusinessResponse<SlackMessage> IsCommandValid(ref Dto.Command command)
        {
            var resp = new BusinessResponse<SlackMessage>()
            {
                ResponseCode = ResponseCode.Fail
            };

            //check is cmd exist. If true set all cmd properties from db.
            var cmdExistanceResp = CheckCommand(ref command);

            if (cmdExistanceResp.ResponseCode != ResponseCode.Success)
            {
                resp.ResponseCode = cmdExistanceResp.ResponseCode;

                var errorMessage = GetSlackMessage(command.Responses.First(p => p.Language == _team.Language).ErrorText);
                resp.ResponseData = errorMessage;

                return resp;
            }

            //check if team has rights to execute this command.
            var authResp = AuthorizeCommand(ref command);

            if (authResp.ResponseCode != ResponseCode.Success)
            {
                resp.ResponseCode = authResp.ResponseCode;

                var errorMessage = GetSlackMessage(command.Responses.First(p => p.Language == _team.Language).ErrorText);
                resp.ResponseData = errorMessage;

                return resp;
            }

            resp.ResponseCode = ResponseCode.Success;

            return resp;
        }

        protected void SetTeam(string slackTeamId)
        {
            _team = _teamBusiness.GetTeamBySlackId(slackTeamId);
        }

        protected void SetTeamLanguage(Language l)
        {
            _team.Language = l;
        }

        protected void SetTeamChannel()
        {
            _team.Channel = _channel;
        }

        protected void SetCommands()
        {
            _commands = _cmdBusiness.All();
        }

        protected void SetChannel(string channel)
        {
            _channel = channel;
        }

        /// <summary>
        /// returns first success message with desired language
        /// </summary>
        /// <returns></returns>
        protected SlackMessage GetSlackExecutionSuccessMessage()
        {
            return new SlackMessage
            {
                text = ExecutingCommand.Responses.First(p => p.Language == _team.Language && p.Depth == 0).SuccessText,
                token = _team.BotAccessToken,
                channel = _channel
            };
        }

        /// <summary>
        /// returns first success message with desired language and depth
        /// </summary>
        /// <returns></returns>
        protected SlackMessage GetSlackExecutionSuccessMessage(int depth)
        {
            return new SlackMessage
            {
                text = ExecutingCommand.Responses.First(p => p.Language == _team.Language && p.Depth == depth).SuccessText,
                token = _team.BotAccessToken,
                channel = _channel
            };
        }

        /// <summary>
        /// returns success message with given text
        /// </summary>
        /// <returns></returns>
        protected SlackMessage GetSlackExecutionSuccessMessage(string text)
        {
            return new SlackMessage
            {
                text = text,
                token = _team.BotAccessToken,
                channel = _channel
            };
        }

        /// <summary>
        /// returns error text with desired language and depth
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        protected SlackMessage GetSlackExecutionErrorMessage(int depth)
        {
            return new SlackMessage
            {
                text = ExecutingCommand.Responses.First(p => p.Language == _team.Language && p.Depth == depth).ErrorText,
                token = _team.BotAccessToken,
                channel = _channel
            };
        }

        protected BusinessResponse<int> ValidateParameters(IEnumerable<CommandParameter> commandParameters, int parameterCount)
        {
            var resp = new BusinessResponse<int>
            {
                ResponseCode = ResponseCode.Fail
            };

            if (Parameters == null || !Parameters.Any())
            {
                resp.ResponseData = 0; // parameter not found.
                return resp;
            }

            if (Parameters.Length != parameterCount)
            {
                resp.ResponseData = 1; // parameter length mismatch.
                return resp;
            }

            if (commandParameters != null)
            {
                foreach (var item in commandParameters)
                {
                    if (!item.ParameterSet.Contains(item.ParameterValue))
                    {
                        // parameter not found in parameter set
                        resp.ResponseData = item.Depth;
                        return resp;
                    }
                }
            }

            resp.ResponseCode = ResponseCode.Success;
            return resp;
        }

        /// <summary>
        /// checks and sets command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private BusinessResponse CheckCommand(ref Dto.Command command)
        {
            var resp = new BusinessResponse
            {
                ResponseCode = ResponseCode.NoContent
            };

            string commandText;

            var array = command.Text
                .ToLower()
                .Replace("  ", " ") // more than one space char is unaccaptable
                .Split(' '); //split by space char

            if (array.Length <= 2)
            {
                if (array[0] == "set" && array[1] == "channel")
                {
                    commandText = "set channel";  //special case for set channel cmd
                }
                else
                    commandText = array[0]; //one word cmd (like 'help') or 2 words cmd with 1 parameter
            }
            else
            {
                commandText = $"{array[0]} {array[1]}"; //cmd has only two words max
            }

            command = _commands.FirstOrDefault(p => p.Text == commandText);

            if (command == null)
            {
                command = _commands.First(p => p.Id == DatabaseKey.Command.CheckCommandExistance);
                return resp;
            }

            resp.ResponseCode = ResponseCode.Success;
            return resp;
        }

        /// <summary>
        /// authorizes command.
        /// checks if team has rights to execute this command.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private BusinessResponse AuthorizeCommand(ref Dto.Command command)
        {
            var resp = new BusinessResponse
            {
                ResponseCode = ResponseCode.Unauthorized
            };

            var scopeId = command.ScopeId;

            var teamScope = _team.TeamScopes.FirstOrDefault(p => p.ScopeId == scopeId);

            if (teamScope == null)
            {
                command = _commands.First(p => p.Id == DatabaseKey.Command.Authorize);
                return resp;
            }

            resp.ResponseCode = ResponseCode.Success;
            return resp;
        }

        private SlackMessage GetSlackMessage(string text)
        {
            return new SlackMessage
            {
                text = text,
                token = _team.BotAccessToken,
                channel = _channel
            };
        }

    }
}