using MoneyMarket.Common;
using MoneyMarket.Common.Response;
using MoneyMarket.DataAccess;
using MoneyMarket.Dto;

namespace MoneyMarket.Business.Account
{
    public class AccountBusiness
    {
        private readonly UserDataAccess _dataAccess = new UserDataAccess();

        public BusinessResponse<string> CreateUser(ApplicationUser userDto, string password, bool isAuthor = false)
        {
            var resp = new BusinessResponse<string> { ResponseCode = ResponseCode.Fail };

            var dbResp = _dataAccess.CreateUser(userDto, password, isAuthor);

            if (dbResp.ResponseCode != ResponseCode.Success)
            {
                resp.ResponseMessage = dbResp.ResponseMessage;
                return resp;
            }

            resp.ResponseCode = ResponseCode.Success;
            resp.ResponseData = dbResp.ResponseData;

            return resp;
        }

        public ApplicationUser GetUserInfo(string userId)
        {
            return _dataAccess.GetUserInfo(userId);
        }

        public int Edit(ApplicationUser user)
        {
            return _dataAccess.Edit(user);
        }

        public BusinessResponse<ApplicationUser> GetUserByEmailOrUserName(string emailOrUsername)
        {
            var resp = new BusinessResponse<ApplicationUser>
            {
                ResponseCode = ResponseCode.Fail,
                ResponseData = _dataAccess.GetUserByEmailOrUserName(emailOrUsername)
            };

            if (resp.ResponseData == null)
            {
                resp.ResponseMessage = ErrorMessage.UserOrEmailNotFound;
                return resp;
            }

            resp.ResponseCode = ResponseCode.Success;
            return resp;
        }

        public BusinessResponse ResetPassword(string userId, string newPassword)
        {
            var resp = new BusinessResponse
            {
                ResponseCode = ResponseCode.Fail
            };

            var dataAccessResp = _dataAccess.ResetPassword(userId, newPassword);

            if (dataAccessResp.ResponseCode != ResponseCode.Success)
            {
                resp.ResponseMessage = ErrorMessage.UserOrEmailNotFound;
                return resp;
            }

            resp.ResponseCode = ResponseCode.Success;
            return resp;
        }


        public int GetTotalUsersCount()
        {
            return _dataAccess.GetUserCounts();
        }
    }
}
