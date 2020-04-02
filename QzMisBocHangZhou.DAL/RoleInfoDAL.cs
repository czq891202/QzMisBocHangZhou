using QzMisBocHangZhou.Model;
using System.Collections.Generic;
using System.Data.Common;

namespace QzMisBocHangZhou.DAL
{
    public class RoleInfoDAL
    {
        #region 【Get】
        public static List<Role> Get()
        {
            var sql = "select * from Role order by RoleName";

            return DBCache.DataBase.ExecuteEntityList<Role>(sql);
        }


        public static Role Get(string id)
        {
            var sql = "select * from Role where Id = :Id";

            return DBCache.DataBase.ExecuteEntity<Role>(
                sql,
                DBCache.DataBase.CreatDbParameter("Id", id));
        }


        public static List<RoleValue> GetRoleValue(string roleId)
        {
            var sql = "select * from RoleValue where RoleId = :RoleId";

            return DBCache.DataBase.ExecuteEntityList<RoleValue>(sql,
                DBCache.DataBase.CreatDbParameter("RoleId", roleId));
        }

        #endregion


        #region 【Add】
        public static int Add(Role role, List<RoleValue> roleValues)
        {
            return DBCache.DataBase.ExecuteNonQuery((cmd) =>
            {
                AddRoleInfo(cmd, role);
                AddRoleValueInfo(cmd, roleValues);
                return 1;
            });
        }

        private static int AddRoleInfo(DbCommand cmd, Role role)
        {
            var sql = "insert into Role (Id, RoleName, Remark) values(:Id, :RoleName, :Remark)";
            cmd.CommandText = sql;

            cmd.Parameters.Clear();
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", role.Id));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("RoleName", role.RoleName));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Remark", role.Remark));

            return cmd.ExecuteNonQuery();
        }

        private static int AddRoleValueInfo(DbCommand cmd, List<RoleValue> roleValueList)
        {
            var sql = "insert into RoleValue (Id, RoleId, NavigationId, ActionType) values(:Id, :RoleId, :NavigationId, :ActionType)";
            cmd.CommandText = sql;

            foreach (var roleValue in roleValueList)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", roleValue.Id));
                cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("RoleId", roleValue.RoleId));
                cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("NavigationId", roleValue.NavigationId));
                cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("ActionType", roleValue.ActionType));

                cmd.ExecuteNonQuery();
            }

            return 1;
        }

        #endregion


        #region 【Update】
        public static int Update(Role role, List<RoleValue> roleValues)
        {
            return DBCache.DataBase.ExecuteNonQuery((cmd) =>
            {
                var ret = UpdateRoleInfo(cmd, role);

                if (ret == 0) return ret;

                DelRoleValueInfo(cmd, role.Id);
                AddRoleValueInfo(cmd, roleValues);

                return ret;
            });
        }

        private static int UpdateRoleInfo(DbCommand cmd, Role role)
        {
            var sql = "update Role set RoleName = :RoleName, Remark = :Remark where Id = :Id";
            cmd.CommandText = sql;

            cmd.Parameters.Clear();
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("RoleName", role.RoleName));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Remark", role.Remark));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", role.Id));

            return cmd.ExecuteNonQuery();
        }
        
        #endregion



        #region 【Del】
        public static int Del(string id)
        {
            return DBCache.DataBase.ExecuteNonQuery((cmd) =>
            {
                DelRoleInfo(cmd, id);
                DelRoleValueInfo(cmd, id);
                return 1;
            });
        }


        private static int DelRoleInfo(DbCommand cmd, string id)
        {
            var sql = "delete Role where Id = :Id";
            cmd.CommandText = sql;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", id));
            return cmd.ExecuteNonQuery();
        }


        private static int DelRoleValueInfo(DbCommand cmd, string roleId)
        {
            var sql = "delete RoleValue where RoleId = :RoleId";
            cmd.CommandText = sql;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("RoleId", roleId));
            return cmd.ExecuteNonQuery();
        }

        #endregion
    }
}
