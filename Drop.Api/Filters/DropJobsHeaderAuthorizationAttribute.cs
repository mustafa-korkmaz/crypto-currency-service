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
    public class DropJobsHeaderAuthorizationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            IEnumerable<string> headerValues;

            actionContext.Request.Headers.TryGetValues(RequestHeader.JobApiKey, out headerValues);

            string jobApiKeyHash = string.Empty;

            if (headerValues != null)
            {
                jobApiKeyHash = headerValues.FirstOrDefault();
            }

            if (string.IsNullOrEmpty(jobApiKeyHash))
            {
                // return 401 (Unauthorized)
                actionContext.Response = new HttpResponseMessage
                {
                    Content = new StringContent(ErrorMessage.JobApiKeyNotFound, Encoding.UTF8),
                    StatusCode = HttpStatusCode.BadRequest
                };

                return;
            }

            if (!IsRequestAuthenticated(jobApiKeyHash))
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

        private bool IsRequestAuthenticated(string jobApiKeyHash)
        {
            if (string.IsNullOrEmpty(jobApiKeyHash))
            {
                return false;
            }

            if (jobApiKeyHash != MoneyMarketConstant.JobApiKeyHash)
            {
                return false;
            }
            return true;
        }

    }
}
