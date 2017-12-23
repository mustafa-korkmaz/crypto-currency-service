
namespace MoneyMarket.Common.ApiObjects.Request.User
{
    public class NewEmailVerificationRequest
    {
        public string UserNameOrEmail { get; set; }

        public string SecurityCode { get; set; }

        public string SecurityCodeHash { get; set; }
    }
}

