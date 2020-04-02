using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class AppSession
    {
        private static readonly string m_User = "User";
        private static readonly string m_Role = "Role";
        private static readonly string m_RoleValue = "RoleValue";

        static AppSession()
        {
            HttpContext.Current.Session.Timeout = 60;
        }

        public static bool IsExits()
        {
            return HttpContext.Current.Session[m_User] != null &&
                HttpContext.Current.Session[m_Role] != null &&
                HttpContext.Current.Session[m_RoleValue] != null;
        }

        public static void AddUser(UserListViewModel user)
        {
            Add(m_User, user);
        }

        public static UserListViewModel GetUser()
        {
            return HttpContext.Current.Session[m_User] as UserListViewModel;
        }

        public static void AddRole(Role role)
        {
            Add(m_Role, role);
        }

        public static Role GetRole()
        {
            return HttpContext.Current.Session[m_Role] as Role;
        }


        public static void AddRoleValue(List<RoleValue> roleValueList)
        {
            Add(m_RoleValue, roleValueList);
        }

        public static List<RoleValue> GetRoleValue()
        {
            return HttpContext.Current.Session[m_RoleValue] as List<RoleValue>;
        }


        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }


        private static void Add(string name, object data)
        {
            
            HttpContext.Current.Session.Add(name, data);
        }


        private static void RemoveAll()
        {
            HttpContext.Current.Session.RemoveAll();
        }


        private static void Remove(string name)
        {
            HttpContext.Current.Session.Remove(name);
        }


    }
}