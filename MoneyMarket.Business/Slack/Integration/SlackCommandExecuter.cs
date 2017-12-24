using MoneyMarket.Common;
using MoneyMarket.Common.Response;

namespace MoneyMarket.Business.Slack.Integration
{
    public abstract class SlackCommandExecuter
    {
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

        protected virtual BusinessResponse<SlackMessage> IsCommandValid(Dto.Command command)
        {
            var resp = new BusinessResponse<SlackMessage>
            {
                ResponseCode = ResponseCode.Fail
            };


            return resp;
        }
    }
}