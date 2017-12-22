
namespace MoneyMarket.Common.ApiObjects.Request.SlackApp
{
    public class OAuthRequest
    {
        public string client_id { get; set; }

        public string client_secret { get; set; }

        public string code { get; set; }

        public string redirect_uri { get; set; }
    }
}