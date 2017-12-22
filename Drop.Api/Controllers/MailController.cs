using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using MoneyMarket.Api.Filters;
using MoneyMarket.Business.Account;
using MoneyMarket.Business.Mailing;
using MoneyMarket.Common;
using MoneyMarket.Common.ApiObjects.Request.User;
using MoneyMarket.Common.Helper;
using MoneyMarket.Common.Response;

namespace MoneyMarket.Api.Controllers
{
    /// <summary>
    /// User mail operations controller.
    /// </summary>
    [ExceptionHandler]
    [RoutePrefix("Mail")]
    public class MailController : ApiController
    {
        /// <summary>
        /// NewEmailVerification
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IHttpActionResult NewEmailVerification([FromBody]NewEmailVerificationRequest request)
        {
            var resp = SendNewVerificationMail(request);
            if (resp.ResponseCode != ResponseCode.Success)
            {
                return BadRequest(resp.ResponseMessage);
            }
            return Ok(resp);
        }

        /// <summary>
        /// reset password via reminder email
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IHttpActionResult ResetPassword([FromBody]ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors(ModelState));
            }

            var apiResp = new ApiResponse()
            {
                ResponseCode = ResponseCode.Fail
            };

            if (!ValidateToken(request.UserId, request.Token))
            {
                return BadRequest(ErrorMessage.WrongVerificationLink);
            }

            // so far so good, now reset password

            var business = new AccountBusiness();

            var businessRes = business.ResetPassword(request.UserId, request.NewPassword);

            if (businessRes.ResponseCode != ResponseCode.Success)
            {
                return BadRequest(businessRes.ResponseMessage);
            }

            return Ok(apiResp);
        }

        private ApiResponse SendNewVerificationMail(NewEmailVerificationRequest request)
        {
            var apiResp = new ApiResponse
            {
                ResponseCode = ResponseCode.Fail
            };

            var captchaValue = StringCipher.Decrypt(request.SecurityCodeHash, MoneyMarketConstant.EncyrptingPassword);

            if (captchaValue != request.SecurityCode)
            {
                apiResp.ResponseMessage = ErrorMessage.WrongSecurityCode;
                return apiResp;
            }

            var mailBusiness = new MailBusiness();
            var accountBusiness = new AccountBusiness();

            var accountResp = accountBusiness.GetUserByEmailOrUserName(request.UserNameOrEmail);

            if (accountResp.ResponseCode != ResponseCode.Success)
            {
                apiResp.ResponseMessage = accountResp.ResponseMessage;
                return apiResp;
            }

            var mailResp = mailBusiness.SendVerificationMail(accountResp.ResponseData.UserName, accountResp.ResponseData.Email);

            if (mailResp.ResponseCode != ResponseCode.Success)
            {
                apiResp.ResponseMessage = mailResp.ResponseMessage;
                return apiResp;
            }

            apiResp.ResponseCode = ResponseCode.Success;
            return apiResp;
        }

        #region Helpers

        private bool ValidateToken(string userId, string encryptedToken)
        {
            var decyrptedToken = StringCipher.Decrypt(encryptedToken, MoneyMarketConstant.EncyrptingPassword);

            if (decyrptedToken == null)
            {
                return false;
            }

            var tokenParam = decyrptedToken.Split(':')[0];

            return tokenParam == userId; //is userId=token.userId ?
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
