using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.DAL
{
    public class RoleValueDAL
    {
        public static List<RoleValue> GetByRoleId(string roleId)
        {
            var sql = "select * from RoleValue where RoleId = :RoleId";

            return DBCache.DataBase.ExecuteEntityList<RoleValue>(sql,
                DBCache.DataBase.CreatDbParameter("RoleId", roleId));
        }


        public static RoleValue Get(string id)
        {
            var sql = "select * from RoleValue where Id = :Id;";

            return DBCache.DataBase.ExecuteEntity<RoleValue>(
                sql,
                DBCache.DataBase.CreatDbParameter("Id", id));
        }


        public static int Add(RoleValue roleValue)
        {
            var sql = "insert into RoleValue (Id, RoleId, NavigationId, ActionType) values(:Id, :RoleId, :NavigationId, :ActionType)";
            return DBCache.DataBase.ExecuteNonQuery(sql,
                DBCache.DataBase.CreatDbParameter("Id", roleValue.Id),
                DBCache.DataBase.CreatDbParameter("RoleId", roleValue.RoleId),
                DBCache.DataBase.CreatDbParameter("NavigationId", roleValue.NavigationId),
                DBCache.DataBase.CreatDbParameter("ActionType", roleValue.ActionType));
        }



        public static int Del(string roleValueId)
        {
            var sql = "delete from RoleValue where Id = :Id";
            return DBCache.DataBase.ExecuteNonQuery(sql, DBCache.DataBase.CreatDbParameter("Id", roleValueId));
        }
    }
}
