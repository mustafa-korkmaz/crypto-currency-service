
namespace MoneyMarket.Business.Slack.Integration
{
    public class SlackMessage
    {

        public string token { get; set; }
        public string channel { get; set; }

        private string _text;
        public string text
        {
            get => _text.Replace("{lf}", "\n");
            set => _text = value;
        }
    }
}
