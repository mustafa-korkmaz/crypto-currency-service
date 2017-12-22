namespace MoneyMarket.Common.ApiObjects.Request.User
{
    public class LoginRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string grant_type { get; set; }
    }
}

