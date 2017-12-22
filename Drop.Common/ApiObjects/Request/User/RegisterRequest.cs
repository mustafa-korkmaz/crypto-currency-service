
namespace MoneyMarket.Common.ApiObjects.Request.User
{
    public class RegisterRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string NameSurname { get; set; }

        public string Email { get; set; }
    }
}
