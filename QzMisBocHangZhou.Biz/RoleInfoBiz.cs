using QzMisBocHangZhou.DAL;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;

namespace QzMisBocHangZhou.Biz
{
    public class RoleInfoBiz
    {
        public static List<Role> Get()
        {
            return RoleInfoDAL.Get();
        }

        public static Role Get(string id)
        {
            if (string.IsNullOrEmpty(id)) return new Role();

            return RoleInfoDAL.Get(id);
        }


        public static List<RoleValue> GetRoleValue(string roleId)
        {
            if (string.IsNullOrEmpty(roleId)) return new List<RoleValue>();

            return RoleInfoDAL.GetRoleValue(roleId);
        }


        public static bool Add(Role data, List<string> navIds)
        {
            data.Id = Guid.NewGuid().ToString();
            if (!RequiredData(data)) return false;

            return RoleInfoDAL.Add(data, BuildRoleValueData(data.Id, navIds)) > 0;
        }


        public static bool Update(Role data, List<string> navIds)
        {
            if (!RequiredData(data)) return false;
            return RoleInfoDAL.Update(data, BuildRoleValueData(data.Id, navIds)) > 0;
        }

        private static List<RoleValue> BuildRoleValueData(string roleId, List<string> navIds)
        {
            var result = new List<RoleValue>();
            if (navIds == null) return result;
            navIds.Remove("-1");
            foreach (var id in navIds)
            {
                result.Add(new RoleValue()
                {
                    Id = Guid.NewGuid().ToString(),
                    RoleId = roleId,
                    NavigationId = id,
                    ActionType = ""
                });

            }
            return result;
        }


        public static bool Del(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            RoleInfoDAL.Del(id);
            return true;
        }


        private static bool RequiredData(Role role)
        {
            if (role == null) return false;
            if (string.IsNullOrWhiteSpace(role.Id)) return false;
            if (string.IsNullOrWhiteSpace(role.RoleName)) return false;

            return true;
        }
    }
}
