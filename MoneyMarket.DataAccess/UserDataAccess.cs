using System;
using System.Linq;
using System.Security.Cryptography;
using MoneyMarket.Common;
using MoneyMarket.Common.Response;
using MoneyMarket.DataAccess.Models;

namespace MoneyMarket.DataAccess
{
    public class UserDataAccess
    {
        private readonly MoneyDbContext _db = new MoneyDbContext();

        public DataAccessResponse<string> CreateUser(Dto.ApplicationUser userDto, string password, bool isAuthor)
        {
            var resp = new DataAccessResponse<string> { ResponseCode = ResponseCode.Fail };

            bool userExists = true;

            if (string.IsNullOrEmpty(userDto.Email))
            {
                userExists = _db.Users.Any(u => u.UserName == userDto.UserName);
            }
            else
                userExists = _db.Users.Any(u => u.UserName == userDto.UserName || u.Email == userDto.Email);

            if (userExists)
            {
                resp.ResponseMessage = ErrorMessage.UserExists;
                return resp;
            }

            var userModel = new ApplicationUser
            {
                NameSurname = userDto.NameSurname,
                Email = userDto.Email ?? "",
                EmailConfirmed = userDto.EmailConfirmed,
                PhoneNumber = userDto.PhoneNumber,
                UserName = userDto.UserName,
                Status = userDto.Status,
                TwoFactorEnabled = userDto.ContactPermission, // contact permission field matched with twoFactorEnabled column
                PasswordHash = HashPassword(password),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var newUser = _db.Users.Add(userModel);

            //if (isAuthor)
            //{
            //    var role = new IdentityUserRole
            //    {
            //        UserId = newUser.Id,
            //        RoleId = DatabaseKey.ApplicationRoleId.Author
            //    };
            //    newUser.Roles.Add(role);
            //}
            _db.SaveChanges();

            resp.ResponseCode = ResponseCode.Success;
            resp.ResponseData = newUser.Id;

            return resp;
        }

        public Dto.ApplicationUser GetUserInfo(string userId)
        {
            var query = _db.Users.FirstOrDefault(u => u.Id == userId);

            if (query == null)
            {
                return null;
            }

            var user = new Dto.ApplicationUser
            {
                Id = userId,
                NameSurname = query.NameSurname,
                Email = query.Email,
                UserName = query.UserName,
                PhoneNumber = query.PhoneNumber
            };

            return user;
        }

        public int Edit(Dto.ApplicationUser user)
        {
            var userModel = _db.Users.Single(u => u.Id == user.Id);

            userModel.NameSurname = user.NameSurname;
            //userModel.Email = user.Email;
            //userModel.PhoneNumber = user.PhoneNumber;
            //userModel.Status = user.Status;

            _db.Entry(userModel).State = System.Data.Entity.EntityState.Modified;

            return _db.SaveChanges();
        }

        public int ConfirmEmail(string userName, string email)
        {
            var userModel = _db.Users.Single(u => u.UserName == userName && u.Email == email);

            userModel.EmailConfirmed = true;

            _db.Entry(userModel).State = System.Data.Entity.EntityState.Modified;

            return _db.SaveChanges();
        }

        public bool IsUserExists(string userName, string email)
        {
            return _db.Users.Any(u => u.UserName == userName && u.Email == email);
        }

        public Dto.ApplicationUser GetUserByEmailOrUserName(string emailOrUsername)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName == emailOrUsername || u.Email == emailOrUsername);

            if (user != null)
                return new Dto.ApplicationUser
                {
                    Id = user.Id,
                    Email = user.Email,
                    NameSurname = user.NameSurname,
                    UserName = user.UserName
                };

            return null;
        }

        public DataAccessResponse ResetPassword(string userId, string password)
        {
            var resp = new DataAccessResponse
            {
                ResponseCode = ResponseCode.Fail
            };

            var userModel = _db.Users.FirstOrDefault(u => u.Id == userId);

            if (userModel == null)
            {
                return resp;
            }

            userModel.PasswordHash = HashPassword(password);

            _db.Entry(userModel).State = System.Data.Entity.EntityState.Modified;

            _db.SaveChanges();

            resp.ResponseCode = ResponseCode.Success;

            return resp;
        }

        public int GetUserCounts()
        {
            return _db.Users.Count();
        }

        /// <summary>
        /// only use when password change request
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserPasswordHash(string userId)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return null;
            }

            return user.PasswordHash;
        }

        //public int SaveUserDevice(DTO.UserDevice userDevice)
        //{
        //    var userDeviceEntities = _db.UserDevices.Where(u => u.UserId == userDevice.UserId && u.Type == userDevice.Type).ToList();

        //    var userDeviceKeys = userDeviceEntities.Select(p => p.Key);

        //    if (userDeviceKeys.Contains(userDevice.Key))
        //    { // this key already exists
        //        return 0;
        //    }

        //    // add new device
        //    var newUserDeviceEntity = new UserDevice
        //    {
        //        UserId = userDevice.UserId,
        //        Key = userDevice.Key,
        //        Type = userDevice.Type
        //    };

        //    _db.UserDevices.Add(newUserDeviceEntity);

        //    return _db.SaveChanges();
        //}

        //public int DeleteUserDevice(DTO.UserDevice userDevice)
        //{
        //    var userDeviceEntities = _db.UserDevices.Where(u => u.UserId == userDevice.UserId && u.Type == userDevice.Type && u.Key == userDevice.Key).ToList();

        //    if (userDeviceEntities.Count == 0)
        //    { // this key already has been deleted or never existed
        //        return 0;
        //    }

        //    _db.UserDevices.RemoveRange(userDeviceEntities);

        //    return _db.SaveChanges();
        //}

        //public int SaveUserRequest(DTO.UserRequest userRequest)
        //{
        //    var userRequestEntity = new UserRequest
        //    {
        //        UserId = userRequest.UserId,
        //        Message = userRequest.Message,
        //        Type = userRequest.Type,
        //        CreatedAt = DateTime.Now
        //    };

        //    _db.UserRequests.Add(userRequestEntity);

        //    return _db.SaveChanges();
        //}

        #region helpers

        #region password hashing & verifying

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }

        public static bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }

        #endregion password hashing & verifying

        #endregion helpers
    }
}
