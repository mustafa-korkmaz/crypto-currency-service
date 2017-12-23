
namespace MoneyMarket.Common.ApiObjects.Request.SlackApp
{
    public class SlackEventSubscriptionRequest
    {
        public string Token { get; set; }

        public string TeamId { get; set; }

        public string ApiAppId { get; set; }

        public string Challenge { get; set; }

        public string Type { get; set; }

        public string EventId { get; set; }

        public SlackEvent Event { get; set; }

    }

    public class SlackEvent
    {
        public string Text { get; set; }

        public string User { get; set; }

        public string Inviter { get; set; }

        public string BotId { get; set; }

        public string Type { get; set; }

        public string SubType { get; set; }

        public string Ts { get; set; }

        public string Channel { get; set; }

        public string EventTs { get; set; }
    }
}