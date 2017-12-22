namespace MoneyMarket.Common.ApiObjects.Response.SlackApp
{
    public class OAuthResponse
    {
        public bool ok { get; set; }

        public string error { get; set; }

        public string bot_access_token { get; set; }

        public string team_name { get; set; }

        public string team_id { get; set; }

        public Bot bot { get; set; }
    }

    public class Bot
    {
        public string bot_user_id { get; set; }

        public string bot_access_token { get; set; }
    }
}

/*
{
    "ok": true,
    "access_token": "xoxp-287240013153-287796683620-287969731474-1eb8fcdcadd5ddcedc8e639849846c90",
    "scope": "identify,bot,chat:write:bot",
    "user_id": "U8FPEL3J8",
    "team_name": "test1",
    "team_id": "T8F720D4H",
    "bot": {
        "bot_user_id": "U8GQKTUF8",
        "bot_access_token": "xoxb-288835946518-a1l4LnxuzSUWfActY1I4eDHD"
    }
} 
*/
