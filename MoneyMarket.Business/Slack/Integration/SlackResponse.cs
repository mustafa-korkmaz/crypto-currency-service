
namespace MoneyMarket.Business.Slack.Integration
{
    public class SlackResponse
    {
        public bool ok { get; set; }
        public string error { get; set; }
        public SlackResponseMessage message { get; set; }
    }

    public class SlackResponseMessage
    {
        public string text { get; set; }

        public string type { get; set; }

        public string sub_type { get; set; }

        public string username { get; set; }
    }
}
