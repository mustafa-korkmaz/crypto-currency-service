using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using MoneyMarket.Business.Exception;
using MoneyMarket.Common;

namespace MoneyMarket.Api.Filters
{
    /// <summary>
    /// Handles all exceptions
    /// </summary>
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// handles all un expected api errors
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var expBusiness = new ExceptionBusiness();

            expBusiness.SaveExceptionAsync(context.Exception);

            context.Response.Content = new StringContent(ErrorMessage.ApplicationExceptionMessage); // default error message for client
        }
    }
}
