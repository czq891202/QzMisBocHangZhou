using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QzMisBocHangZhou.DAL;
using QzMisBocHangZhou.Model;

namespace QzMisBocHangZhou.Biz
{
    public class UserInfoBiz
    {
        public static bool Login(string userName, string password)
        {
            return UserInfoDAL.Login(userName, password) > 0;
        }

        public static List<UserListViewModel> Get()
        {
            return UserInfoDAL.Get();
        }

        public static UserInfo Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return new UserInfo();
            return UserInfoDAL.Get(id);
        }

        public static UserListViewModel GetDetailsByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return new UserListViewModel();
            return UserInfoDAL.GetDetailsByName(name);
        }

        public static bool SetUserStatus(string id, int status)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            return UserInfoDAL.SetUserStatus(id, status) > 0;
        }

        public static bool ChangePassword(string id, string password)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            return UserInfoDAL.ChangePassword(id, password) > 0;
        }

        public static bool ChangeMobile(string id, string mobile)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            return UserInfoDAL.ChangeMobile(id, mobile) > 0;
        }

        public static bool Add(UserInfo data)
        {
            data.Id = Guid.NewGuid().ToString();
            if (!RequiredData(data)) return false;
            return UserInfoDAL.Add(data) > 0;
        }

        public static bool Update(UserInfo data)
        {
            if (!RequiredData(data)) return false;
            return UserInfoDAL.Update(data) > 0;
        }
        /// <summary>
        /// 设置最后一次登陆时间
        /// </summary>
        /// <param name="data">用户信息</param>
        /// <returns></returns>
        public static bool SetLastLandingTime(UserInfo data)
        {
            if (!RequiredData(data)) return false;
            return UserInfoDAL.SetLastLandingTime(data) > 0;
        }

        public static bool IsExistsUser(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return false;
            return UserInfoDAL.GetUserNameCount(userName) > 0;
        }

        private static bool RequiredData(UserInfo data)
        {
            if (data == null) return false;
            if (string.IsNullOrWhiteSpace(data.Id)) return false;
            if (string.IsNullOrWhiteSpace(data.UserName)) return false;
            if (string.IsNullOrWhiteSpace(data.Password)) return false;
            if (string.IsNullOrWhiteSpace(data.OrgId)) return false;
            if (string.IsNullOrWhiteSpace(data.RoleId)) return false;
            if (string.IsNullOrWhiteSpace(data.RealName)) return false;
            return true;
        }
    }
}
