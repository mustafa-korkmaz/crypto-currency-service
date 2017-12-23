using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MoneyMarket.Common;

namespace MoneyMarket.Api.Filters
{
    /// <summary>
    /// Authorizes the header values. returns 400 if request lacks required header values
    /// </summary>
    public class MoneyMarketHeaderAuthorizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var slackActionRequest = actionContext.ActionArguments.FirstOrDefault();

            if (slackActionRequest.Value != null)
            {
                var slackActionRequestStr = slackActionRequest.Value.ToString();

                var slackRequest = "MoneyMarket.Common.ApiObjects.Request.SlackApp.SlackAppRequest";

                if (slackActionRequestStr == slackRequest)
                {
                    // dont validate headers for slack reqs
                    base.OnActionExecuting(actionContext);
                    return;
                }
            }

            IEnumerable<string> headerValues;

            actionContext.Request.Headers.TryGetValues(RequestHeader.ChannelType, out headerValues);

            if (headerValues == null)
            {
                // return 401 (Unauthorized)
                actionContext.Response = new HttpResponseMessage
                {
                    Content = new StringContent(ErrorMessage.ChannelTypeNotFound, Encoding.UTF8),
                    StatusCode = HttpStatusCode.BadRequest
                };

                return;
            }

            string channel = headerValues.FirstOrDefault();

            if (channel != ChannelType.Android &&
                (channel != ChannelType.Ios) &&
                (channel != ChannelType.LandingPage) &&
                (channel != ChannelType.WebApp))
            {
                // return 401 (Unauthorized)
                actionContext.Response = new HttpResponseMessage
                {
                    Content = new StringContent(ErrorMessage.ChannelTypeIncorrect, Encoding.UTF8),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            actionContext.Request.Headers.TryGetValues(RequestHeader.ApiKey, out headerValues);

            string apiKeyHash = string.Empty;

            if (headerValues != null)
            {
                apiKeyHash = headerValues.FirstOrDefault();
            }

            if (string.IsNullOrEmpty(apiKeyHash))
            {
                // return 401 (Unauthorized)
                actionContext.Response = new HttpResponseMessage
                {
                    Content = new StringContent(ErrorMessage.ApiKeyNotFound, Encoding.UTF8),
                    StatusCode = HttpStatusCode.BadRequest
                };

                return;
            }

            if (!IsRequestAuthenticated(apiKeyHash))
            {
                // return 401 (Unauthorized)
                actionContext.Response = new HttpResponseMessage
                {
                    Content = new StringContent(ErrorMessage.ApiKeyIncorrect, Encoding.UTF8),
                    StatusCode = HttpStatusCode.BadRequest
                };

            }
            else
                base.OnActionExecuting(actionContext);
        }

        private bool IsRequestAuthenticated(string apiKeyHash)
        {
            if (string.IsNullOrEmpty(apiKeyHash))
            {
                return false;
            }

            if (apiKeyHash != MoneyMarketConstant.ApiKeyHash)
            {
                return false;
            }
            return true;
        }

    }
}
