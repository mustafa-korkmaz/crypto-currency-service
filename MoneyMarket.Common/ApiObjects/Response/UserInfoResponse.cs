using System;
using System.Collections.Generic;

namespace MoneyMarket.Common.ApiObjects.Response
{
    public class UserInfoResponse
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string NameSurname { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<UserClaim> Claims { get; set; }
        public DateTime JoinedAt { get; set; }
    }

    public class UserClaim
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

}

