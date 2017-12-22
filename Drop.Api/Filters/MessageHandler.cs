using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MoneyMarket.Business.Logging;
using MoneyMarket.Common.Helper;
using MoneyMarket.Dto;

namespace MoneyMarket.Api.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MessageHandler : DelegatingHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestContent = await request.Content.ReadAsStringAsync();

            var response = await base.SendAsync(request, cancellationToken);

            string pathAndQuery = request.RequestUri.PathAndQuery;
            var url = pathAndQuery.Length >= 49 ? pathAndQuery.Substring(0, 49) : pathAndQuery;

            this.ProcessMessageAsync((int)response.StatusCode, requestContent, url);

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseStatusCode"></param>
        /// <param name="requestContent"></param>
        /// <param name="absolutePath"></param>
        protected abstract void ProcessMessageAsync(int responseStatusCode, string requestContent, string absolutePath);
    }

    /// <summary>
    /// 
    /// </summary>
    public class MessageLoggingHandler : MessageHandler
    {
        /// <summary>
        /// saves req and resp async
        /// </summary>
        /// <param name="responseStatusCode"></param>
        /// <param name="requestContent"></param>
        /// <param name="url"></param>
        protected override void ProcessMessageAsync(int responseStatusCode, string requestContent, string url)
        {
            //trim requestContent as max 500 char variable
            var content = string.IsNullOrEmpty(requestContent)
                ? null
                : requestContent.Length >= 500 ? requestContent.Substring(0, 500) : requestContent;

            var requestLog = new RequestLog
            {
                Ip = Statics.GetIp(),
                HttpResponseCode = responseStatusCode,
                RequestContent = content,
                Uri = url,
                CreatedAt = Statics.GetTurkeyCurrentDateTime()
            };

            var loggingBusiness = new LoggingBusiness();

            loggingBusiness.LogRequestAsync(requestLog);
        }
    }
}