
using System;

namespace MoneyMarket.Common.ApiObjects.Response
{
    public class LoginResponse
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string NameSurname { get; set; }

        public string Email { get; set; }

        public string AccessToken { get; set; }

        public string PhoneNumber { get; set; }

        public int CoinsTotal { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
