using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MoneyMarket.Common;

namespace MoneyMarket.Api.Filters
{
    /// <summary>
    /// Handles user claims
    /// </summary>
    public class ClaimsHandlerAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// comma seperated parameters for attribute
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// While claims handler method executing 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var resp = new
            {
                Message = ErrorMessage.ClaimsNotFound
            };

            var jsonResp = Newtonsoft.Json.JsonConvert.SerializeObject(resp);

            var claimParameters = Parameters.Split(',').ToList();

            ClaimsPrincipal principal = actionContext.Request.GetRequestContext().Principal as ClaimsPrincipal;

            if (principal == null)
            {
                //return 400(BadRequest)
                actionContext.Response = new HttpResponseMessage
                {
                    Content = new StringContent(jsonResp, Encoding.UTF8),
                    StatusCode = HttpStatusCode.BadRequest
                };

                return;
            }
            var claims = principal.Claims;

            bool hasUserClaim = false;
            foreach (var claim in claimParameters)
            {
                hasUserClaim = claims.Any(c => c.Value == claim);

                if (hasUserClaim)
                {
                    break;
                }
            }

            if (!hasUserClaim)
            {
                // return 400 (BadRequest)
                actionContext.Response = new HttpResponseMessage
                {
                    Content = new StringContent(jsonResp, Encoding.UTF8),
                    StatusCode = HttpStatusCode.BadRequest
                };

                return;
            }

            base.OnActionExecuting(actionContext);
        }

    }
}