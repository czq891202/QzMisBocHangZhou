using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;

namespace QzMisBocHangZhou.DAL
{
    public class UserInfoDAL
    {
        public static int Login(string userName, string pwd)
        {
            var sql = "select count(1) from UserInfo where UserName = :UserName and Password = :Password and Status = 0";

            return DBCache.DataBase.ExecuteScalar<int>(
                sql,
                DBCache.DataBase.CreatDbParameter("UserName", userName),
                DBCache.DataBase.CreatDbParameter("Password", pwd));
        }

        public static int ChangePassword(string id, string password)
        {
            var sql = "update UserInfo set Password = :Password where Id = :Id";
            return DBCache.DataBase.ExecuteNonQuery(sql,
                DBCache.DataBase.CreatDbParameter("Password", password),
                DBCache.DataBase.CreatDbParameter("Id", id));
        }

        public static int ChangeMobile(string id, string mobile)
        {
            var sql = "update UserInfo set Mobile = :Mobile where Id = :Id";
            return DBCache.DataBase.ExecuteNonQuery(sql,
                DBCache.DataBase.CreatDbParameter("Mobile", mobile),
                DBCache.DataBase.CreatDbParameter("Id", id));
        }

        public static List<UserListViewModel> Get()
        {
            var sql = @"select u.*, r.rolename,(select wm_concat(to_char(name)) from OrgInfo o where o.Id in (select column_value from table (split (u.OrgId)))) AS OrgName from UserInfo u 
                        left join Role r on u.roleid = r.id";
            return DBCache.DataBase.ExecuteEntityList<UserListViewModel>(sql);
        }

        public static UserListViewModel GetDetailsByName(string name)
        {
            var sql = @"select u.*, o.name as OrgName, r.rolename,o.parentid as ParentOrgId from UserInfo u 
                        left join OrgInfo o on u.orgid = o.id 
                        left join Role r on u.roleid = r.id 
                        where u.UserName = :UserName";

            return DBCache.DataBase.ExecuteEntity<UserListViewModel>(
                sql,
                DBCache.DataBase.CreatDbParameter("UserName", name));
        }

        public static UserInfo Get(string id)
        {
            var sql = "select * from UserInfo where Id = :Id";

            return DBCache.DataBase.ExecuteEntity<UserInfo>(
                sql,
                DBCache.DataBase.CreatDbParameter("Id", id));
        }

        public static int Add(UserInfo user)
        {
            var sql = "insert into UserInfo (Id, UserName, Password, OrgId, RoleId, RealName, Mobile, Status) values(:Id, :UserName, :Password, :OrgId, :RoleId, :RealName, :Mobile, :Status)";
            return DBCache.DataBase.ExecuteNonQuery(sql,
                DBCache.DataBase.CreatDbParameter("Id", user.Id),
                DBCache.DataBase.CreatDbParameter("UserName", user.UserName),
                DBCache.DataBase.CreatDbParameter("Password", user.Password),
                DBCache.DataBase.CreatDbParameter("OrgId", user.OrgId),
                DBCache.DataBase.CreatDbParameter("RoleId", user.RoleId),
                DBCache.DataBase.CreatDbParameter("RealName", user.RealName),
                DBCache.DataBase.CreatDbParameter("Mobile", user.Mobile),
                DBCache.DataBase.CreatDbParameter("Status", user.Status));
        }

        public static int Update(UserInfo user)
        {
            var sql = "update UserInfo set OrgId = :OrgId, RoleId = :RoleId, RealName = :RealName, Mobile = :Mobile, Status = :Status where Id = :Id";
            return DBCache.DataBase.ExecuteNonQuery(sql,
                DBCache.DataBase.CreatDbParameter("OrgId", user.OrgId),
                DBCache.DataBase.CreatDbParameter("RoleId", user.RoleId),
                DBCache.DataBase.CreatDbParameter("RealName", user.RealName),
                DBCache.DataBase.CreatDbParameter("Mobile", user.Mobile),
                DBCache.DataBase.CreatDbParameter("Status", user.Status),
                DBCache.DataBase.CreatDbParameter("Id", user.Id));
        }

        public static int SetUserStatus(string userId, int status)
        {
            var sql = "update UserInfo set Status = :Status where Id = :Id";
            return DBCache.DataBase.ExecuteNonQuery(sql,
                DBCache.DataBase.CreatDbParameter("Status", status),
                DBCache.DataBase.CreatDbParameter("Id", userId));
        }

        public static int GetUserNameCount(string userName)
        {
            var sql = "select count(1) from UserInfo where UserName = :UserName";
            return DBCache.DataBase.ExecuteScalar<int>(
                sql,
                DBCache.DataBase.CreatDbParameter("UserName", userName));
        }

        public static int SetLastLandingTime(UserInfo user)
        {
            var sql = "update UserInfo set LastLandingTime = :LastLandingTime where Id = :Id";
            return DBCache.DataBase.ExecuteNonQuery(sql,
                DBCache.DataBase.CreatDbParameter("LastLandingTime", DateTime.Now),
                DBCache.DataBase.CreatDbParameter("Id", user.Id));
        }
    }
}
