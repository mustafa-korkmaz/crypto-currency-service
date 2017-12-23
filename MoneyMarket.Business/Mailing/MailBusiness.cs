using System.Threading.Tasks;
using MoneyMarket.Common;
using MoneyMarket.Common.Helper;
using MoneyMarket.Common.Response;
using MoneyMarket.DataAccess;
using MoneyMarket.Dto;

namespace MoneyMarket.Business.Mailing
{
    public class MailBusiness
    {
        public BusinessResponse SendVerificationMail(string userName, string toAdress)
        {
            var businessResp = new BusinessResponse
            {
                ResponseCode = ResponseCode.Fail
            };

            var mailHandler = MailHandler.Instance;

            mailHandler.Subject = Mail.VerificationSubject;
            mailHandler.Content = string.Format(Mail.VerificationContent, SetEmailVerificationLink(userName, toAdress));
            mailHandler.Footer = Mail.Footer;

            if (mailHandler.Send(toAdress))
            {
                businessResp.ResponseCode = ResponseCode.Success;
            }
            else
            {
                businessResp.ResponseMessage = ErrorMessage.EmailSendingFailure;
            }
            return businessResp;
        }

        public Task SendVerificationMailAsync(string userName, string toAdress)
        {
            return Task.Run(() =>
            {
                var mailHandler = MailHandler.Instance;
                mailHandler.Subject = Mail.VerificationSubject;
                mailHandler.Content = string.Format(Mail.VerificationContent, SetEmailVerificationLink(userName, toAdress));
                mailHandler.Send(toAdress);
            });
        }

        public BusinessResponse VerifyUser(string verificationLink)
        {
            var businessResp = new BusinessResponse
            {
                ResponseCode = ResponseCode.Fail
            };

            var userNameAndPassword = StringCipher.Decrypt(verificationLink, MoneyMarketConstant.EncyrptingPassword);

            if (userNameAndPassword == null)
            {
                businessResp.ResponseMessage = ErrorMessage.WrongVerificationLink;
                return businessResp;
            }

            var userName = userNameAndPassword.Split(':')[0];
            var email = userNameAndPassword.Split(':')[1];

            var dataAccess = new UserDataAccess();

            dataAccess.ConfirmEmail(userName, email);

            businessResp.ResponseCode = ResponseCode.Success;
            return businessResp;
        }

        public bool IsUserExists(string userName, string email)
        {
            return new UserDataAccess().IsUserExists(userName, email);
        }

        public Task SendReminderMailAsync(ApplicationUser user)
        {
            string userNameWithAtChar = "@" + user.UserName;
            return Task.Run(() =>
            {
                var mailHandler = MailHandler.Instance;
                mailHandler.Subject = Mail.ReminderSubject;
                mailHandler.Content = string.Format(Mail.ReminderContent, userNameWithAtChar, SetPasswordRemiderLink(user.Id, user.Email));
                mailHandler.Footer = Mail.Footer;
                mailHandler.Send(user.Email);
            });
        }

        private string SetEmailVerificationLink(string userName, string email)
        {
            string encryptedstring = StringCipher.Encrypt(userName + ":" + email, MoneyMarketConstant.EncyrptingPassword);
            return Statics.GetApiUrl() + "User/Verify/" + encryptedstring;
        }

        private string SetPasswordRemiderLink(string userId, string email)
        {
            string encryptedstring = StringCipher.Encrypt(userId + ":" + email, MoneyMarketConstant.EncyrptingPassword);
            return Statics.GetApiUrl() + "User/ResetPassword/" + encryptedstring;
        }

    }
}