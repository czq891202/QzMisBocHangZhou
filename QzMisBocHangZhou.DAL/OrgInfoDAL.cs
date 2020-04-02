using QzMisBocHangZhou.Model;
using System.Collections.Generic;

namespace QzMisBocHangZhou.DAL
{
    public class OrgInfoDAL
    {
        public static List<OrgInfo> GetAll()
        {
            var sql = @"select t.*, Level 
                        from OrgInfo t 
                        start with t.parentid = '-1' 
                        connect by prior t.id = t.parentid 
                        order by level, t.Code";

            return DBCache.DataBase.ExecuteEntityList<OrgInfo>(sql);
        }


        //public static List<OrgInfo> Get()
        //{
        //    var sql = @"select t.*, Level 
        //                from OrgInfo t 
        //                where t.IsLock = 0 
        //                start with t.parentid = '-1' 
        //                connect by prior t.id = t.parentid 
        //                order by level, t.Code";

        //    return DBCache.DataBase.ExecuteEntityList<OrgInfo>(sql);
        //}


        public static OrgInfo Get(string id)
        {
            var sql = "select * from OrgInfo where Id = :Id";

            return DBCache.DataBase.ExecuteEntity<OrgInfo>(sql,
                DBCache.DataBase.CreatDbParameter("Id", id));
        }


        public static OrgInfo GetByCode(string code)
        {
            var sql = "select * from OrgInfo where Code = :Code";

            return DBCache.DataBase.ExecuteEntity<OrgInfo>(sql,
                DBCache.DataBase.CreatDbParameter("Code", code));
        }


        public static List<OrgInfo> GetChild(string id)
        {
            var sql = @"select t.*, Level 
                        from OrgInfo t 
                        where t.IsLock = 0 
                        start with Id = :Id 
                        connect by prior t.id = t.parentid 
                        order by level, t.Code";

            return DBCache.DataBase.ExecuteEntityList<OrgInfo>(sql, DBCache.DataBase.CreatDbParameter("Id", id));
        }


        public static int Add(OrgInfo orgInfo)
        {
            var sql = @"insert into OrgInfo (Id, ParentId, TypeName, Name, Code, IsLock, Remark, Contact, ShortName) 
                        values(:Id, :ParentId, :TypeName, :Name, :Code, :IsLock, :Remark, :Contact, :ShortName)";

            return DBCache.DataBase.ExecuteNonQuery(sql,
                    DBCache.DataBase.CreatDbParameter("Id", orgInfo.Id),
                    DBCache.DataBase.CreatDbParameter("ParentId", orgInfo.ParentId),
                    DBCache.DataBase.CreatDbParameter("TypeName", orgInfo.TypeName),
                    DBCache.DataBase.CreatDbParameter("Name", orgInfo.Name),
                    DBCache.DataBase.CreatDbParameter("Code", orgInfo.Code),
                    DBCache.DataBase.CreatDbParameter("IsLock", orgInfo.IsLock),
                    DBCache.DataBase.CreatDbParameter("Remark", orgInfo.Remark),
                    DBCache.DataBase.CreatDbParameter("Contact", orgInfo.Contact),
                    DBCache.DataBase.CreatDbParameter("ShortName", orgInfo.ShortName)
                );
        }


        public static int Update(OrgInfo orgInfo)
        {
            var sql = @"update OrgInfo set ParentId = :ParentId, TypeName = : TypeName, Name = :Name, Code = :Code, IsLock = :IsLock, 
                        Remark = :Remark, Contact = :Contact, ShortName = :ShortName
                        where Id = :Id";
            return DBCache.DataBase.ExecuteNonQuery(sql,
                    DBCache.DataBase.CreatDbParameter("ParentId", orgInfo.ParentId),
                    DBCache.DataBase.CreatDbParameter("TypeName", orgInfo.TypeName),
                    DBCache.DataBase.CreatDbParameter("Name", orgInfo.Name),
                    DBCache.DataBase.CreatDbParameter("Code", orgInfo.Code),
                    DBCache.DataBase.CreatDbParameter("IsLock", orgInfo.IsLock),
                    DBCache.DataBase.CreatDbParameter("Remark", orgInfo.Remark),
                    DBCache.DataBase.CreatDbParameter("Contact", orgInfo.Contact),
                    DBCache.DataBase.CreatDbParameter("ShortName", orgInfo.ShortName),
                    DBCache.DataBase.CreatDbParameter("Id", orgInfo.Id));
        }


        public static int Disable(string id)
        {
            var sql = @"update OrgInfo set IsLock = 1 where Id in (select Id from OrgInfo start with Id = :Id connect by prior Id = ParentId)";
            return DBCache.DataBase.ExecuteNonQuery(sql, DBCache.DataBase.CreatDbParameter("Id", id));
        }


        public static int Enable(string id)
        {
            var sql = @"update OrgInfo set IsLock = 0 where Id in (select Id from OrgInfo start with Id = :Id connect by prior ParentId = Id)";
            return DBCache.DataBase.ExecuteNonQuery(sql, DBCache.DataBase.CreatDbParameter("Id", id));
        }
    }
}
