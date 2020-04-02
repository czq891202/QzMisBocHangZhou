using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.DAL
{
    public class InventoryInfoDAL
    {
        public static PagingResult<ArchiveInfo> GetInventoryArchiveList(int page, int limit, string orgId)
        {
            var sql = @"select tpa.*, org.Code as OrgCode, org.Name as OrgName from Archiveinfo tpa 
                        left join orginfo org on tpa.OrgId = org.Id  where 1=1 ";

            sql += $" and (STATUS = {ArchiveStatusType.已入库.GetHashCode()}) ";

            var pars = new List<DbParameter>();
            if (!orgId.Equals(OrgInfo.RootId, StringComparison.OrdinalIgnoreCase))
            {
                sql += @" and tpa.OrgId = :OrgId ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            sql += " order by tpa.CREATEDATE, tpa.ORGID desc ";
            var rCount = DBCache.DataBase.GetRecordCount(sql, pars.ToArray());
            var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveInfo>(page, limit, sql, pars.ToArray());

            return new PagingResult<ArchiveInfo>() { Count = rCount, Result = data };
        }


        public static List<InventoryDetail> GetInventoryArchiveList(string orgId)
        {
            var sql = $"select tpa.Id as ArchiveId, tpa.LabelCode from Archiveinfo tpa where (STATUS = {ArchiveStatusType.已入库.GetHashCode()}) ";

            var pars = new List<DbParameter>();
            if (!orgId.Equals(OrgInfo.RootId, StringComparison.OrdinalIgnoreCase))
            {
                sql += @" and tpa.OrgId = :OrgId ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            sql += " order by tpa.CREATEDATE, tpa.ORGID desc ";
            return DBCache.DataBase.ExecuteEntityList<InventoryDetail>(sql, pars.ToArray());
        }


        public static InventoryInfo Get(string id)
        {
            var sql = @"select * from inventoryinfo where Id = :Id";

            return DBCache.DataBase.ExecuteEntity<InventoryInfo>(sql, DBCache.DataBase.CreatDbParameter("Id", id));
        }


        public static PagingResult<InventoryInfo> Get(int page, int limit)
        {
            var sql = @"select t.*, d1.SuccessCount, d2.Total from inventoryinfo t  
                        left join (select inventoryid, count(1) as SuccessCount from inventorydetail d where status = 1 group by inventoryid) d1 on t.id = d1.inventoryid
                        left join (select inventoryid, count(1) as Total from inventorydetail d group by inventoryid) d2 on t.id = d2.inventoryid
                        order by t.StartTime desc";

            var rCount = DBCache.DataBase.GetRecordCount(sql);
            var data = DBCache.DataBase.ExecuteEntityListByPageing<InventoryInfo>(page, limit, sql);

            return new PagingResult<InventoryInfo>() { Count = rCount, Result = data };
        }


        public static List<InventoryDetail> GetInventoryDetails(string tId)
        {
            var sql = @"select t.Id, a.LabelCode from inventorydetail t
                        left join ArchiveInfo a on t.archiveid = a.id where t.InventoryId = :InventoryId";
            return DBCache.DataBase.ExecuteEntityList<InventoryDetail>(sql, DBCache.DataBase.CreatDbParameter("InventoryId", tId));
        }


        public static int Add(InventoryInfo info, List<InventoryDetail> details)
        {
            return DBCache.DataBase.ExecuteNonQuery((cmd) =>
            {
                var ret = AddInventoryInfo(cmd, info);
                ret += AddInventoryDetail(cmd, details);
                return ret;
            });
        }

        private static int AddInventoryInfo(DbCommand cmd, InventoryInfo data)
        {
            if (data == null) return 1;

            var sql = "insert into InventoryInfo (Id, UserId, OrgId, InventoryName, StartTime, EndTime, Remark, IsLocked) values(:Id, :UserId, :OrgId, :InventoryName, :StartTime, :EndTime, :Remark, :IsLocked)";
            cmd.CommandText = sql;

            cmd.Parameters.Clear();
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", data.Id));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("UserId", data.UserId));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("OrgId", data.OrgId));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("InventoryName", data.InventoryName));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("StartTime", data.StartTime));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("EndTime", data.EndTime));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Remark", data.Remark));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("IsLocked", data.IsLocked));

            return cmd.ExecuteNonQuery();
        }


        private static int AddInventoryDetail(DbCommand cmd, List<InventoryDetail> data)
        {
            if (data == null || data.Count == 0) return 1;

            var ret = 0;
            var sql = "insert into InventoryDetail (Id, InventoryId, ArchiveId, InventoryTime, Status) values(:Id, :InventoryId, :ArchiveId, :InventoryTime, :Status)";
            cmd.CommandText = sql;

            foreach (var item in data)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", item.Id));
                cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("InventoryId", item.InventoryId));
                cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("ArchiveId", item.ArchiveId));
                cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("InventoryTime", item.InventoryTime));
                cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", item.Status.GetHashCode()));

                ret += cmd.ExecuteNonQuery();
            }

            return ret;
        }


        public static int ImportInventoryData(InventoryInfo data, List<InventoryDetail> details)
        {
            return DBCache.DataBase.ExecuteNonQuery((cmd) =>
            {
                var ret = ImportInventoryInfoData(cmd, data);
                ret += ImportInventoryDetailsData(cmd, details);
                return ret;
            });
        }

        private static int ImportInventoryInfoData(DbCommand cmd, InventoryInfo data)
        {
            if (data.IsLocked != 2) return 1;
            var sql = @"update InventoryInfo set IsLocked = 2, EndTime = :EndTime where Id = :Id";
            cmd.CommandText = sql;

            cmd.Parameters.Clear();
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("EndTime", data.EndTime));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", data.Id));

            return cmd.ExecuteNonQuery();
        }


        private static int ImportInventoryDetailsData(DbCommand cmd, List<InventoryDetail> details)
        {
            if (details == null || details.Count == 0) return 1;
            var sql = @"update InventoryDetail set Status = :Status where Id = :Id";
            cmd.CommandText = sql;

            var ret = 0;
            foreach (var item in details)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", item.Status.GetHashCode()));
                cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", item.Id));
                ret += cmd.ExecuteNonQuery();
            }

            return ret;
        }


        public static PagingResult<InventoryDetail> GetDetails(int page, int limit, string tId)
        {
            var sql = @"select t.*, a.QuotaNo, a.LoanAccount, a.LabelCode from inventorydetail t
                        left join ArchiveInfo a on t.archiveid = a.id where t.InventoryId = :InventoryId";
            var rCount = DBCache.DataBase.GetRecordCount(sql, DBCache.DataBase.CreatDbParameter("InventoryId", tId));
            var data = DBCache.DataBase.ExecuteEntityListByPageing<InventoryDetail>(page, limit, sql, DBCache.DataBase.CreatDbParameter("InventoryId", tId));

            return new PagingResult<InventoryDetail>() { Count = rCount, Result = data };
        }


        //public static PagingResult<InventoryInfo> Get(int page, int limit)
        //{
        //    var sql = @"select t.*, d1.SuccessCount, d2.Total from inventoryinfo t  
        //                left join (select inventoryid, count(1) as SuccessCount from inventorydetail d where status = 1 group by inventoryid) d1 on t.id = d1.inventoryid
        //                left join (select inventoryid, count(1) as Total from inventorydetail d group by inventoryid) d2 on t.id = d2.inventoryid
        //                order by t.StartTime desc";

        //    var rCount = DBCache.DataBase.GetRecordCount(sql);
        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<InventoryInfo>(page, limit, sql);

        //    return new PagingResult<InventoryInfo>() { Count = rCount, Result = data };
        //}


        //public static PagingResult<InventoryInfo> Get(int page, int limit, string year)
        //{
        //    var sql = @"select t.*, d1.SuccessCount, d2.Total from inventoryinfo t  
        //                left join (select inventoryid, count(1) as SuccessCount from inventorydetail d where status = 1 group by inventoryid) d1 on t.id = d1.inventoryid
        //                left join (select inventoryid, count(1) as Total from inventorydetail d group by inventoryid) d2 on t.id = d2.inventoryid
        //                where TO_CHAR(t.starttime,'yyyy') = :Year order by t.StartTime desc";

        //    var rCount = DBCache.DataBase.GetRecordCount(sql, DBCache.DataBase.CreatDbParameter("Year", year));
        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<InventoryInfo>(page, limit, sql, DBCache.DataBase.CreatDbParameter("Year", year));

        //    return new PagingResult<InventoryInfo>() { Count = rCount, Result = data };
        //}





        //public static int Add(InventoryInfo data, List<InventoryDetail> details)
        //{
        //    return DBCache.DataBase.ExecuteNonQuery((cmd) =>
        //    {
        //        var ret = AddInventoryInfo(cmd, data);
        //        ret += AddInventoryDetail(cmd, details);
        //        return ret;
        //    });
        //}


        //private static int AddInventoryInfo(DbCommand cmd, InventoryInfo data)
        //{
        //    if (data == null) return 1;

        //    var sql = "insert into InventoryInfo (Id, UserId, OrgId, InventoryName, StartTime, EndTime, Remark, IsLocked) values(:Id, :UserId, :OrgId, :InventoryName, :StartTime, :EndTime, :Remark, :IsLocked)";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", data.Id));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("UserId", data.UserId));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("OrgId", data.OrgId));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("InventoryName", data.InventoryName));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("StartTime", data.StartTime));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("EndTime", data.EndTime));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Remark", data.Remark));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("IsLocked", data.IsLocked));

        //    return cmd.ExecuteNonQuery();
        //}


        //private static int AddInventoryDetail(DbCommand cmd, List<InventoryDetail> data)
        //{
        //    if (data == null || data.Count == 0) return 1;

        //    var ret = 0;
        //    var sql = "insert into InventoryDetail (Id, InventoryId, ArchiveId, InventoryTime, Status) values(:Id, :InventoryId, :ArchiveId, :InventoryTime, :Status)";
        //    cmd.CommandText = sql;

        //    foreach (var item in data)
        //    {
        //        cmd.Parameters.Clear();
        //        cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", item.Id));
        //        cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("InventoryId", item.InventoryId));
        //        cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("ArchiveId", item.ArchiveId));
        //        cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("InventoryTime", item.InventoryTime));
        //        cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", item.Status.GetHashCode()));

        //        ret += cmd.ExecuteNonQuery();
        //    }

        //    return ret;
        //}


        //public static int Del(string id)
        //{
        //    return DBCache.DataBase.ExecuteNonQuery((cmd) =>
        //    {
        //        var ret = DelInfo(cmd, id);
        //        ret += DelDetail(cmd, id);
        //        return ret;
        //    });
        //}


        //private static int DelInfo(DbCommand cmd, string id)
        //{
        //    var sql = "delete from InventoryInfo where Id = :Id";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", id));

        //    return cmd.ExecuteNonQuery();
        //}


        //private static int DelDetail(DbCommand cmd, string id)
        //{
        //    var sql = "delete from InventoryDetail where InventoryId = :InventoryId";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("InventoryId", id));

        //    return cmd.ExecuteNonQuery();
        //}


        //public static int DelDetail(string inventoryId, string archiveId)
        //{
        //    var sql = "delete from InventoryDetail where InventoryId = :InventoryId and ArchiveId = :ArchiveId";
        //    return DBCache.DataBase.ExecuteNonQuery(sql,
        //        DBCache.DataBase.CreatDbParameter("InventoryId", inventoryId),
        //        DBCache.DataBase.CreatDbParameter("ArchiveId", archiveId));
        //}


        //public static PagingResult<SelectArchiveModel> GetArchiveList(int page, int limit, string inventoryId)
        //{
        //    var sql = @"select a.*, b.IsChecked, o.name as OrgName, o.code as OrgCode from ArchiveInfo a 
        //                left join (select ArchiveId, 1 as IsChecked from InventoryDetail where InventoryId = :InventoryId) b on a.id = b.archiveid
        //                left join orginfo o on a.orgid = o.id
        //                order by  b.IsChecked, a.orgid, a.quotano, a.loanaccount";

        //    var rCount = DBCache.DataBase.GetRecordCount(sql, DBCache.DataBase.CreatDbParameter("InventoryId", inventoryId));
        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<SelectArchiveModel>(page, limit, sql, DBCache.DataBase.CreatDbParameter("InventoryId", inventoryId));

        //    return new PagingResult<SelectArchiveModel>() { Count = rCount, Result = data };
        //}



        //public static int Update(InventoryInfo data)
        //{
        //    var sql = @"update InventoryInfo set InventoryName = :InventoryName, StartTime = :StartTime, Remark = :Remark where Id = :Id";
        //    return DBCache.DataBase.ExecuteNonQuery(sql,
        //        DBCache.DataBase.CreatDbParameter("InventoryName", data.InventoryName),
        //        DBCache.DataBase.CreatDbParameter("StartTime", data.StartTime),
        //        DBCache.DataBase.CreatDbParameter("Remark", data.Remark),
        //        DBCache.DataBase.CreatDbParameter("Id", data.Id));
        //}




        //public static int LockInfo(string id)
        //{
        //    var sql = @"update InventoryInfo set IsLocked = 0 where Id = :Id";
        //    return DBCache.DataBase.ExecuteNonQuery(sql,
        //        DBCache.DataBase.CreatDbParameter("Id", id));
        //}

    }
}
