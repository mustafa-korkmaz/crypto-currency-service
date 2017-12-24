using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// slack team fulfilled by event subscription request
        /// </summary>
        private Dto.Team _team;

        private string _channel;

        /// <summary>
        /// scope= chat:general
        /// cmd= 'hi'
        /// </summary>
        /// <returns></returns>
        protected abstract BusinessResponse<SlackResponse> SayHello();

        /// <summary>
        /// posts command list to channel.
        /// scope= chat:general
        /// cmd= 'help'
        /// </summary>
        /// <returns></returns>
        protected abstract BusinessResponse<SlackResponse> Help();

        /// <summary>
        /// scope= set:settings
        /// cmd= 'l set @p0'
        /// </summary>
        /// <param name="culture">@p0 parameter for desired language</param>
        /// <returns></returns>
        protected abstract BusinessResponse<SlackResponse> SetLanguage(string culture);

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

        protected void SetCommands()
        {
            _commands = _cmdBusiness.All();
        }

        protected void SetChannel(string channel)
        {
            _channel = channel;
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

            var commandText = command.Text;

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