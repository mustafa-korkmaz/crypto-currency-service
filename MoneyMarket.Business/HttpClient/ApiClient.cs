using System.Net;
using System.Text;
using MoneyMarket.Common;
using MoneyMarket.Common.Helper;
using MoneyMarket.Common.Response;

namespace MoneyMarket.Business.HttpClient
{
    public class ApiClient
    {
        /// <summary>
        /// default http client
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public HttpClientResponse<string> GetWebResponse(string url)
        {
            var resp = new HttpClientResponse<string>();

            WebClient client = new WebClient();

            client.Encoding = Encoding.UTF8;
            client.Headers.Add("User-Agent: Other");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                string html = client.DownloadString(url);
                if (!string.IsNullOrEmpty(html))
                {
                    resp.ResponseData = html;
                    resp.HttpStatusCode = HttpStatusCode.OK;
                }
                else
                {
                    resp.HttpStatusCode = HttpStatusCode.BadRequest;
                    resp.ResponseMessage = HttpClientResponseMessage.ContentNoFound;
                }
            }
            catch (WebException exc)
            {
                var excResp = exc.Response as HttpWebResponse;

                if (excResp == null)
                {
                    resp.HttpStatusCode = HttpStatusCode.InternalServerError;
                    return resp;
                }

                resp.HttpStatusCode = excResp.StatusCode;
            }

            return resp;
        }
    }
}