using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MoneyMarket.Api.Filters;
using MoneyMarket.Api.Models;
using MoneyMarket.Business.Account;
using MoneyMarket.Business.Mailing;
using MoneyMarket.Common;
using MoneyMarket.Common.ApiObjects.Response;
using MoneyMarket.Common.Response;
using MoneyMarket.Dto;
using DefaultAuthenticationTypes = Microsoft.AspNet.Identity.DefaultAuthenticationTypes;

namespace MoneyMarket.Api.Controllers
{
    /// <summary>
    /// User details and account operations controller.
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [MoneyMarketHeaderAuthorization]  // needs api key  and channelType
    [Authorize]
    [ExceptionHandler]
    [RoutePrefix("")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";

        private readonly AccountBusiness _accountBusiness = new AccountBusiness();

        /// <summary>
        /// default constructor
        /// </summary>
        public AccountController()
        {
        }

        /// <summary>
        /// token access constructor
        /// </summary>
        /// <param name="accessTokenFormat"></param>
        public AccountController(ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            AccessTokenFormat = accessTokenFormat;
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        /// <summary>
        /// Returns user info who has already signed in.
        /// </summary>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("User")]
        public IHttpActionResult GetUserInfo()
        {
            var user = _accountBusiness.GetUserInfo(User.Identity.GetUserId());

            if (user == null)
            {
                return BadRequest(ErrorMessage.RecordNotFound);
            }

            var resp = new ApiResponse<UserInfoResponse>() { ResponseCode = ResponseCode.Fail };

            var userInfo = new UserInfoResponse
            {
                Email = user.Email,
                NameSurname = user.NameSurname,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                UserId = user.Id,
            };

            var principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            if (principal != null)
            {
                var claims = principal.Claims;
                userInfo.Claims = GetUserClaims(claims);
            }

            resp.ResponseData = userInfo;
            resp.ResponseCode = ResponseCode.Success;

            return Ok(resp);
        }

        /// <summary>
        /// New user registration method.
        /// </summary>
        /// <param name="model">Register binding model</param>
        /// <returns>User info with a guid userId</returns>
        [AllowAnonymous]
        [Route("Register")]
        public IHttpActionResult Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors(ModelState));
            }

            var resp = new ApiResponse<ApplicationUser>
            {
                ResponseCode = ResponseCode.Fail
            };

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                NameSurname = model.NameSurname,
                //PhoneNumber = model.PhoneNumber,
                //ContactPermission = model.ContactPermission
            };

            var userCreateResp = _accountBusiness.CreateUser(user, model.Password);

            if (userCreateResp.ResponseCode != ResponseCode.Success)
            {
                return BadRequest(userCreateResp.ResponseMessage);
            }

            user.Id = userCreateResp.ResponseData;

            //if (channelType != ChannelType.WebAdmin) // mobile request
            //{
            //    if (!string.IsNullOrEmpty(model.DeviceKey))
            //    {
            //        _accountBusiness.SaveUserDevice(user.Id, model.DeviceKey, channelType);
            //    }

            //    // fire & forget email
            //    new MailBusiness().SendVerificationMailAsync(model.UserName, model.Email);
            //}

            // fire & forget email
            new MailBusiness().SendVerificationMailAsync(model.UserName, model.Email);

            resp.ResponseCode = ResponseCode.Success;
            resp.ResponseData = user;

            return Ok(resp);
        }

        /// <summary>
        /// sends email for forgotten passwords
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Remind")]
        public IHttpActionResult Remind(RemindPaswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors(ModelState));
            }

            var apiResp = SendReminderEmail(model.EmailOrUserName);

            if (apiResp.ResponseCode != ResponseCode.Success)
            {
                return BadRequest(apiResp.ResponseMessage);
            }

            return Ok(apiResp);
        }


        /// <summary>
        /// saves device key for mobile notifications
        /// </summary>
        /// <returns></returns>
        //[Route("SaveDevice")]
        //public IHttpActionResult SaveDevice(UserDeviceModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(GetModelStateErrors(ModelState));
        //    }

        //    var channelType = GetChannelType();

        //    var apiResp = SaveUserDevice(model.DeviceKey, channelType);

        //    if (apiResp.ResponseCode != ResponseCode.Success)
        //    {
        //        return BadRequest(apiResp.ResponseMessage);
        //    }

        //    return Ok(apiResp);

        //}

        /// <summary>
        /// mails request, suggestion or complimant for app
        /// </summary>
        /// <returns></returns>
        //[Route("RequestBox")]
        //public IHttpActionResult SaveRequest(UserRequestModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(GetModelStateErrors(ModelState));
        //    }

        //    var apiResp = SaveUserRequest(model);

        //    if (apiResp.ResponseCode != ResponseCode.Success)
        //    {
        //        return BadRequest(apiResp.ResponseMessage);
        //    }

        //    return Ok(apiResp);
        //}

        /// <summary>
        /// logout operation for mobile users
        /// </summary>
        /// <returns></returns>
        [Route("Logout")]
        public IHttpActionResult Logout(LogoutModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(GetModelStateErrors(ModelState));
            //}

            //var channelType = GetChannelType();

            //var apiResp = LogoutUser(model.DeviceKey, channelType);

            //if (apiResp.ResponseCode != ResponseCode.Success)
            //{
            //    return BadRequest(apiResp.ResponseMessage);
            //}

            //return Ok(apiResp);
            return Ok();
        }

        [Route("totalUserCounts")]
        [AllowAnonymous]
        public int GetTotalUsersCount()
        {
            return _accountBusiness.GetTotalUsersCount();
        }

        #region Helpers

        private IEnumerable<UserClaim> GetUserClaims(IEnumerable<Claim> claims)
        {
            var claimsModel = new List<UserClaim>();

            foreach (var claim in claims)
            {
                if (claim.Type.Contains("http")) // do not get asp.net default claims
                {
                    if (claim.Type.Contains("identity/claims/role"))
                    {
                        var roleClaimModel = new UserClaim
                        {
                            Type = ApplicationUserClaimType.Role,
                            Value = claim.Value
                        };
                        claimsModel.Add(roleClaimModel);
                    }
                    continue;
                }

                var claimModel = new UserClaim
                {
                    Type = claim.Type,
                    Value = claim.Value
                };
                claimsModel.Add(claimModel);
            }

            return claimsModel;
        }

        private string GetChannelType()
        {
            IEnumerable<string> headerValues = null;
            Request.Headers.TryGetValues(RequestHeader.ChannelType, out headerValues);

            return headerValues.FirstOrDefault();
        }

        private ApiResponse SendReminderEmail(string emailOrUserName)
        {
            var apiResp = new ApiResponse
            {
                ResponseCode = ResponseCode.Fail
            };

            var businessResp = _accountBusiness.GetUserByEmailOrUserName(emailOrUserName);

            if (businessResp.ResponseCode != ResponseCode.Success)
            {
                apiResp.ResponseMessage = businessResp.ResponseMessage;
                return apiResp;
            }

            var mailBusiness = new MailBusiness();
            var user = businessResp.ResponseData;

            mailBusiness.SendReminderMailAsync(user);

            apiResp.ResponseCode = ResponseCode.Success;

            return apiResp;
        }

        //private ApiResponse SaveUserDevice(string deviceKey, string channelType)
        //{
        //    var apiResp = new ApiResponse
        //    {
        //        ResponseCode = ResponseCode.Fail
        //    };

        //    if (channelType == ChannelType.WebAdmin || channelType == ChannelType.WebApp)
        //    {
        //        apiResp.ResponseMessage = ErrorMessage.ChannelTypeNotAppropriate;

        //        return apiResp;
        //    }

        //    _accountBusiness.SaveUserDevice(User.Identity.GetUserId(), deviceKey, channelType);

        //    apiResp.ResponseCode = ResponseCode.Success;

        //    return apiResp;
        //}

        //private ApiResponse LogoutUser(string deviceKey, string channelType)
        //{
        //    var apiResp = new ApiResponse
        //    {
        //        ResponseCode = ResponseCode.Fail
        //    };

        //    if (channelType == ChannelType.WebAdmin || channelType == ChannelType.WebApp)
        //    {
        //        apiResp.ResponseMessage = ErrorMessage.ChannelTypeNotAppropriate;

        //        return apiResp;
        //    }

        //    _accountBusiness.DeleteUserDevice(User.Identity.GetUserId(), deviceKey, channelType);

        //    apiResp.ResponseCode = ResponseCode.Success;

        //    return apiResp;
        //}

        //private ApiResponse SaveUserRequest(UserRequestModel model)
        //{
        //    var apiResp = new ApiResponse
        //    {
        //        ResponseCode = ResponseCode.Fail
        //    };

        //    _accountBusiness.SaveUserRequest(User.Identity.GetUserId(), model.Message, model.Type);

        //    apiResp.ResponseCode = ResponseCode.Success;

        //    return apiResp;
        //}

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider)
                };

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        private bool ValidateUser(string userId)
        {
            var currentUserId = User.Identity.GetUserId();

            return currentUserId == userId; //is userId=CurrentUserId ?
        }

        /// <summary>
        /// return model state errors as a sentence.
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        protected string GetModelStateErrors(ModelStateDictionary dic)
        {
            var list = dic.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            string result = string.Empty;

            for (int i = 1; i <= list.Count(); i++)
            {
                result += list[i - 1];

                if (i < list.Count())
                {
                    result += " ";
                }
            }

            return result;
        }

        #endregion
    }
}
