using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.DAL
{
    public class NavigationInfoDAL
    {
        public static List<Navigation> Get()
        {
            var sql = @"select t.*, Level 
                        from navigation t 
                        start with t.parentid = '-1' 
                        connect by prior t.id = t.parentid 
                        order by Level, t.OrderCode, t.Name";

            return DBCache.DataBase.ExecuteEntityList<Navigation>(sql);
        }

        public static Navigation Get(string id)
        {
            var sql = "select * from Navigation where Id = :Id";

            return DBCache.DataBase.ExecuteEntity<Navigation>(sql,
                DBCache.DataBase.CreatDbParameter("Id", id));
        }

        public static List<Navigation> GetByRole(string roleId)
        {
            var sql = @"select t.*, Level 
                        from navigation t 
                        where t.id in 
                        (select r.navigationid from rolevalue r where r.RoleId = :RoleId) 
                        start with t.parentid = '-1' 
                        connect by prior t.id = t.parentid 
                        order by Level, t.OrderCode, t.Name";

            return DBCache.DataBase.ExecuteEntityList<Navigation>(sql,
                DBCache.DataBase.CreatDbParameter("RoleId", roleId));
        }

        public static int Add(Navigation navigation)
        {
            var sql = "insert into Navigation (Id, ParentId, Name, Title, LinkUrl, MenuClass, OrderCode, ActionType, IsLock, Remark) values(:Id, :ParentId, :Name, :Title, :LinkUrl, :MenuClass, :OrderCode, :ActionType, :IsLock, :Remark)";
            return DBCache.DataBase.ExecuteNonQuery(sql,
                DBCache.DataBase.CreatDbParameter("Id", navigation.Id),
                DBCache.DataBase.CreatDbParameter("ParentId", navigation.ParentId),
                DBCache.DataBase.CreatDbParameter("Name", navigation.Name),
                DBCache.DataBase.CreatDbParameter("Title", navigation.Title),
                DBCache.DataBase.CreatDbParameter("LinkUrl", navigation.LinkUrl),
                DBCache.DataBase.CreatDbParameter("MenuClass", navigation.MenuClass),
                DBCache.DataBase.CreatDbParameter("OrderCode", navigation.OrderCode),
                DBCache.DataBase.CreatDbParameter("ActionType", navigation.ActionType),
                DBCache.DataBase.CreatDbParameter("IsLock", navigation.IsLock),
                DBCache.DataBase.CreatDbParameter("Remark", navigation.Remark));
        }

        public static int Update(Navigation navigation)
        {
            var sql = @"update Navigation set ParentId = :ParentId, Name = :Name, Title = :Title, LinkUrl = :LinkUrl, MenuClass = :MenuClass, OrderCode = :OrderCode, ActionType = :ActionType, IsLock = :IsLock, Remark = :Remark where Id = :Id";
            return DBCache.DataBase.ExecuteNonQuery(sql,
                DBCache.DataBase.CreatDbParameter("ParentId", navigation.ParentId),
                DBCache.DataBase.CreatDbParameter("Name", navigation.Name),
                DBCache.DataBase.CreatDbParameter("Title", navigation.Title),
                DBCache.DataBase.CreatDbParameter("LinkUrl", navigation.LinkUrl),
                DBCache.DataBase.CreatDbParameter("MenuClass", navigation.MenuClass),
                DBCache.DataBase.CreatDbParameter("OrderCode", navigation.OrderCode),
                DBCache.DataBase.CreatDbParameter("ActionType", navigation.ActionType),
                DBCache.DataBase.CreatDbParameter("IsLock", navigation.IsLock),
                DBCache.DataBase.CreatDbParameter("Remark", navigation.Remark),
                DBCache.DataBase.CreatDbParameter("Id", navigation.Id));
        }

        public static int Del(string id)
        {
            var sql = @"delete from Navigation where Id in (select Id from Navigation start with Id = :Id connect by prior Id = ParentId)";
            return DBCache.DataBase.ExecuteNonQuery(sql, DBCache.DataBase.CreatDbParameter("Id", id));
        }
    }
}
