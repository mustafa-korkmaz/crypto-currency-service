
namespace MoneyMarket.Common.ApiObjects.Request.SlackApp
{
    public class SlackAppRequest
    {
        public string Token { get; set; }

        public string Challenge { get; set; }

        public string Type { get; set; }

        public string Text { get; set; }

        public string Channel { get; set; }

        public string User { get; set; }

        public string Ts { get; set; }
    }
}