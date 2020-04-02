using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.DAL
{
    public class ArchiveSettleInfoDAL
    {
        public static ArchiveSettleInfo Get(string id)
        {
            var sql = @"select * from ArchiveSettleInfo where Id = :Id";

            return DBCache.DataBase.ExecuteEntity<ArchiveSettleInfo>(sql,
                DBCache.DataBase.CreatDbParameter("Id", id));
        }

        public static PagingResult<ArchiveSettleInfo> GetPreSettle(int page, int limit, string orgId, string keyWords)
        {
            var pars = new List<DbParameter>();

            var sql = @"select ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower as LoanBorrower, ai.Id, ai.Status as ArcStatus,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        from ArchiveInfo ai left join OrgInfo o on ai.OrgId = o.Id 
                        where (ai.Status = 1 or ai.Status = 5) and ai.Id not in (select ArchiveId from ArchiveSettleInfo where ArchiveId is not null) ";

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            if (!string.IsNullOrWhiteSpace(keyWords))
            {
                sql += @" and (ai.LoanAccount like :KeyWords
                        or ai.QuotaNo like :KeyWords 
                        or ai.Borrower like :KeyWords 
                        or ai.CustomerNo like :keywords)";

                pars.Add(DBCache.DataBase.CreatDbParameter("KeyWords", $"%{keyWords.Trim()}%"));
            }

            sql += " order by OrgCode, ai.QuotaNo, ai.LoanAccount";


            var rCount = DBCache.DataBase.GetRecordCount(sql, pars.ToArray());
            var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveSettleInfo>(page, limit, sql, pars.ToArray());

            return new PagingResult<ArchiveSettleInfo>() { Count = rCount, Result = data };
        }


        public static PagingResult<ArchiveSettleInfo> GetPreReview(int page, int limit, string orgId, string keyWords)
        {
            var pars = new List<DbParameter>();

            var sql = @"select ast.*, ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower as LoanBorrower,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        From ArchiveSettleInfo ast left join OrgInfo o on ast.OrgId = o.Id 
                        Left join ArchiveInfo ai on ast.ArchiveId = ai.Id
                        where ast.Status = 0 ";

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and ast.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            if (!string.IsNullOrWhiteSpace(keyWords))
            {
                sql += @" and (ai.LoanAccount like :KeyWords
                        or ai.QuotaNo like :KeyWords 
                        or ai.Borrower like :KeyWords 
                        or ai.CustomerNo like :keywords)";

                pars.Add(DBCache.DataBase.CreatDbParameter("KeyWords", $"%{keyWords.Trim()}%"));
            }

            sql += " order by OrgCode, ai.QuotaNo, ai.LoanAccount";


            var rCount = DBCache.DataBase.GetRecordCount(sql, pars.ToArray());
            var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveSettleInfo>(page, limit, sql, pars.ToArray());

            return new PagingResult<ArchiveSettleInfo>() { Count = rCount, Result = data };
        }


        public static PagingResult<ArchiveSettleInfo> GetPreOut(int page, int limit, string orgId, string keyWords)
        {
            var pars = new List<DbParameter>();

            var sql = @"select ast.*, ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower as LoanBorrower,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        From ArchiveSettleInfo ast left join OrgInfo o on ast.OrgId = o.Id 
                        Left join ArchiveInfo ai on ast.ArchiveId = ai.Id
                        where ast.Status = 1 ";

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and ast.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            if (!string.IsNullOrWhiteSpace(keyWords))
            {
                sql += @" and (ai.LoanAccount like :KeyWords
                        or ai.QuotaNo like :KeyWords 
                        or ai.Borrower like :KeyWords 
                        or ai.CustomerNo like :keywords)";

                pars.Add(DBCache.DataBase.CreatDbParameter("KeyWords", $"%{keyWords.Trim()}%"));
            }

            sql += " order by OrgCode, ai.QuotaNo, ai.LoanAccount";


            var rCount = DBCache.DataBase.GetRecordCount(sql, pars.ToArray());
            var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveSettleInfo>(page, limit, sql, pars.ToArray());

            return new PagingResult<ArchiveSettleInfo>() { Count = rCount, Result = data };
        }


        public static int SubmitReview(ArchiveSettleInfo data)
        {
            var sql = @"insert into ArchiveSettleInfo 
                        (Id, OrgId, SettlePeople, Status, SettleDate, UsedBy, ArchiveId) values
                        (:Id, :OrgId, :SettlePeople, :Status, :SettleDate, :UsedBy, :ArchiveId)";
            return DBCache.DataBase.ExecuteNonQuery(sql,
                DBCache.DataBase.CreatDbParameter("Id", data.Id),
                DBCache.DataBase.CreatDbParameter("OrgId", data.OrgId),
                DBCache.DataBase.CreatDbParameter("SettlePeople", data.SettlePeople),
                DBCache.DataBase.CreatDbParameter("Status", 0),
                DBCache.DataBase.CreatDbParameter("SettleDate", data.SettleDate),
                DBCache.DataBase.CreatDbParameter("UsedBy", data.UsedBy),
                DBCache.DataBase.CreatDbParameter("ArchiveId", data.ArchiveId));
            
        }


        /// <summary>
        /// 撤回/驳回
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int RollBack(string id)
        {
            var sql = @"delete ArchiveSettleInfo where Id = :Id ";
            return DBCache.DataBase.ExecuteNonQuery(
                sql,
                DBCache.DataBase.CreatDbParameter("Id", id));
        }


        public static int PassReview(ArchiveSettleInfo data)
        {
            var arcType = ArchiveStatusType.已结清;
            if(data.UsedBy.Equals("变更结清", StringComparison.OrdinalIgnoreCase))
            {
                arcType = ArchiveStatusType.变更结清出库;
            }
            else if (data.UsedBy.Equals("处置结清", StringComparison.OrdinalIgnoreCase))
            {
                arcType = ArchiveStatusType.处置出库;
            }

            return DBCache.DataBase.ExecuteNonQuery((cmd) =>
            {
                var ret = 0;
                ret = SetInfoPass(cmd, data, 1);

                return ret + ChangeArchiveStatus(cmd, data, arcType);
            });
        }


        private static int SetInfoPass(DbCommand cmd, ArchiveSettleInfo data, int status)
        {
            var sql = @"update ArchiveSettleInfo set Status = :Status where Id = :Id ";
            cmd.CommandText = sql;

            cmd.Parameters.Clear();
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", data.Id));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", status));

            return cmd.ExecuteNonQuery();
        }


        private static int ChangeArchiveStatus(DbCommand cmd, ArchiveSettleInfo data, ArchiveStatusType type)
        {
            var sql = @"update ArchiveInfo Set Status = :Status, LastEditDate = :LastEditDate
                        where Id = :Id ";
            cmd.CommandText = sql;

            cmd.Parameters.Clear();
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", ArchiveStatusType.已结清.GetHashCode()));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("LastEditDate", DateTime.Now));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", data.ArchiveId));

            return cmd.ExecuteNonQuery();
        }


        public static int SettleOut(string id)
        {
            var sql = @"update ArchiveSettleInfo set Status = 2 where Id = :Id ";
            return DBCache.DataBase.ExecuteNonQuery(
                sql,
                DBCache.DataBase.CreatDbParameter("Id", id));
        }


        //public static PagingResult<ArchiveSettleInfo> Get(int page, int limit)
        //{
        //    var sql = @"select a.*, o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact 
        //                from ArchiveSettleInfo a left join OrgInfo o on a.OrgId = o.Id 
        //                order by a.Status, OrgCode, a.SettleDate desc";

        //    var rCount = DBCache.DataBase.GetRecordCount(sql);
        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveSettleInfo>(page, limit, sql);

        //    return new PagingResult<ArchiveSettleInfo>() { Count = rCount, Result = data };

        //}


        //public static PagingResult<ArchiveSettleInfo> GetByOrg(int page, int limit, string orgId)
        //{
        //    var sql = @"select a.*, o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact 
        //                from ArchiveSettleInfo a left join OrgInfo o on a.OrgId = o.Id
        //                where OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) 
        //                order by a.Status, OrgCode, a.SettleDate desc";

        //    var rCount = DBCache.DataBase.GetRecordCount(sql, DBCache.DataBase.CreatDbParameter("OrgId", orgId));
        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveSettleInfo>(page, limit, sql,
        //        DBCache.DataBase.CreatDbParameter("OrgId", orgId));

        //    return new PagingResult<ArchiveSettleInfo>() { Count = rCount, Result = data };
        //}


        //public static ArchiveSettleInfo Get(string id)
        //{
        //    var sql = @"select a.*, o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact 
        //                from ArchiveSettleInfo a left join OrgInfo o on a.OrgId = o.Id
        //                where a.Id = :Id order by a.OrgId, a.SettleDate desc";

        //    return DBCache.DataBase.ExecuteEntity<ArchiveSettleInfo>(
        //        sql,
        //        DBCache.DataBase.CreatDbParameter("Id", id));
        //}


        //public static PagingResult<ArchiveSettleInfo> GetApprovaSettleList(int page, int limit)
        //{
        //    var sql = @"select a.*, o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact  
        //                from ArchiveSettleInfo a left join OrgInfo o on a.OrgId = o.Id 
        //                where a.Status <> 0
        //                order by a.Status, OrgCode, a.SettleDate desc";

        //    var rCount = DBCache.DataBase.GetRecordCount(sql);
        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveSettleInfo>(page, limit, sql);

        //    return new PagingResult<ArchiveSettleInfo>() { Count = rCount, Result = data };

        //}


        //public static PagingResult<ArchiveSettleInfo> GetApprovaSettleList(int page, int limit, string orgId)
        //{
        //    var sql = @"select a.*, o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact   
        //                from ArchiveSettleInfo a left join OrgInfo o on a.OrgId = o.Id
        //                where OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) 
        //                and a.Status <> 0
        //                order by a.Status, OrgCode, a.SettleDate desc";

        //    var rCount = DBCache.DataBase.GetRecordCount(sql, DBCache.DataBase.CreatDbParameter("OrgId", orgId));
        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveSettleInfo>(page, limit, sql,
        //        DBCache.DataBase.CreatDbParameter("OrgId", orgId));

        //    return new PagingResult<ArchiveSettleInfo>() { Count = rCount, Result = data };
        //}


        //public static List<ArchiveSettleDetail> GetDetails(string pId)
        //{
        //    var sql = "select t.*, a.QuotaNo, a.LoanAccount, a.LabelCode, a.Borrower from ArchiveSettleDetail t left join archiveinfo a on t.archiveid = a.id where t.PId =:PId ";

        //    return DBCache.DataBase.ExecuteEntityList<ArchiveSettleDetail>(
        //        sql,
        //        DBCache.DataBase.CreatDbParameter("PId", pId));
        //}


        //public static ArchiveSettleDetail GetDetails(string pId, string archiveId)
        //{
        //    var sql = "select t.*, a.QuotaNo, a.LoanAccount, a.LabelCode from ArchiveSettleDetail t left join archiveinfo a on t.archiveid = a.id where t.PId =:PId  and  t.ArchiveId = :ArchiveId ";

        //    return DBCache.DataBase.ExecuteEntity<ArchiveSettleDetail>(
        //        sql,
        //        DBCache.DataBase.CreatDbParameter("PId", pId),
        //        DBCache.DataBase.CreatDbParameter("ArchiveId", archiveId));
        //}


        //public static PagingResult<SelectArchiveModel> GetArchiveList(int page, int limit, string settleId)
        //{
        //    var sql = @"select a.*, b.IsChecked, o.name as OrgName, o.code as OrgCode from ArchiveInfo a 
        //                left join (select archiveid, 1 as IsChecked from ArchiveSettleDetail where pid = :settleId) b on a.id = b.archiveid
        //                left join orginfo o on a.orgid = o.id
        //                where a.id not in(
        //                        select atd.archiveid from  ArchiveSettleDetail atd left join ArchiveSettleInfo at on atd.pid = at.id
        //                        where at.status <> 0 )
        //                    and (a.status = 1)
        //                order by  b.IsChecked, a.orgid, a.quotano, a.loanaccount";

        //    var rCount = DBCache.DataBase.GetRecordCount(sql, DBCache.DataBase.CreatDbParameter("settleId", settleId));
        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<SelectArchiveModel>(page, limit, sql, DBCache.DataBase.CreatDbParameter("settleId", settleId));

        //    return new PagingResult<SelectArchiveModel>() { Count = rCount, Result = data };
        //}


        //public static PagingResult<SelectArchiveModel> GetArchiveList(int page, int limit, string orgId, string settleId)
        //{
        //    var sql = @"select a.*, b.IsChecked, o.name as OrgName, o.code as OrgCode from ArchiveInfo a 
        //                left join (select archiveid, 1 as IsChecked from ArchiveSettleDetail where pid = :settleId) b on a.id = b.archiveid
        //                left join orginfo o on a.orgid = o.id
        //                where a.id not in(
        //                        select atd.archiveid from  ArchiveSettleDetail atd left join ArchiveSettleInfo at on atd.pid = at.id
        //                        where at.status <> 0 )
        //                    and (a.status = 1)
        //                    and a.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) 
        //                order by  b.IsChecked, a.orgid, a.quotano, a.loanaccount";


        //    var rCount = DBCache.DataBase.GetRecordCount(sql,
        //        DBCache.DataBase.CreatDbParameter("settleId", settleId),
        //        DBCache.DataBase.CreatDbParameter("OrgId", orgId));

        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<SelectArchiveModel>(page, limit, sql,
        //        DBCache.DataBase.CreatDbParameter("settleId", settleId),
        //        DBCache.DataBase.CreatDbParameter("OrgId", orgId));

        //    return new PagingResult<SelectArchiveModel>() { Count = rCount, Result = data };
        //}


        //#region 【Add】
        //public static int Add(ArchiveSettleInfo info, List<ArchiveSettleDetail> details)
        //{
        //    return DBCache.DataBase.ExecuteNonQuery((cmd) =>
        //    {
        //        AddInfo(cmd, info);
        //        AddDetails(cmd, details);
        //        return 1;
        //    });
        //}


        //private static int AddInfo(DbCommand cmd, ArchiveSettleInfo info)
        //{
        //    if (info == null) return 1;
        //    var sql = "insert into ArchiveSettleInfo (Id, OrgId, SettlePeople, Status, SettleDate) values(:Id, :OrgId, :SettlePeople, :Status, :SettleDate)";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", info.Id));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("OrgId", info.OrgId));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("SettlePeople", info.SettlePeople));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", 0));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("SettleDate", info.SettleDate));

        //    return cmd.ExecuteNonQuery();
        //}


        //private static int AddDetails(DbCommand cmd, List<ArchiveSettleDetail> detailsList)
        //{
        //    if (detailsList == null || detailsList.Count == 0) return 1;
        //    var sql = "insert into ArchiveSettleDetail (Id, PId, ArchiveId) values(:Id, :PId, :ArchiveId)";
        //    cmd.CommandText = sql;

        //    foreach (var item in detailsList)
        //    {
        //        cmd.Parameters.Clear();
        //        cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", item.Id));
        //        cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("PId", item.PId));
        //        cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("ArchiveId", item.ArchiveId));
        //        cmd.ExecuteNonQuery();
        //    }

        //    return 1;
        //}

        //#endregion


        //#region 【Update】
        //public static int Update(ArchiveSettleInfo info, List<ArchiveSettleDetail> details)
        //{
        //    return DBCache.DataBase.ExecuteNonQuery((cmd) =>
        //    {
        //        UpdateInfo(cmd, info);

        //        return 1;
        //    });
        //}


        //private static int UpdateInfo(DbCommand cmd, ArchiveSettleInfo info)
        //{
        //    var sql = @"update ArchiveSettleInfo set OrgId = :OrgId, SettlePeople = :SettlePeople,  
        //                    SettleDate = :SettleDate, Status = :Status where Id = :Id ";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("OrgId", info.OrgId));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("SettlePeople", info.SettlePeople));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("SettleDate", info.SettleDate));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", info.Status));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", info.Id));

        //    return cmd.ExecuteNonQuery();
        //}


        //#endregion


        //public static int EditDetails(ArchiveSettleDetail data)
        //{
        //    return DBCache.DataBase.ExecuteNonQuery((cmd) =>
        //    {
        //        return AddDetails(cmd, new List<ArchiveSettleDetail>() { data });
        //    });
        //}


        //public static int Del(string id)
        //{
        //    return DBCache.DataBase.ExecuteNonQuery((cmd) =>
        //    {
        //        var ret = 0;
        //        ret = DelInfo(cmd, id);

        //        return DelDetail(cmd, id) + ret;
        //    });
        //}


        //private static int DelInfo(DbCommand cmd, string id)
        //{
        //    var sql = "delete from ArchiveSettleInfo where Id = :Id and Status = 0 ";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", id));
        //    return cmd.ExecuteNonQuery();
        //}


        //private static int DelDetail(DbCommand cmd, string SettleId)
        //{
        //    var sql = "delete from ArchiveSettleDetail where PId = :PId ";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("PId", SettleId));
        //    return cmd.ExecuteNonQuery();
        //}


        //public static int DelDetail(string SettleId, string archiveId)
        //{
        //    var sql = "delete from ArchiveSettleDetail where PId = :PId and ArchiveId = :ArchiveId ";
        //    return DBCache.DataBase.ExecuteNonQuery(
        //        sql,
        //        DBCache.DataBase.CreatDbParameter("PId", SettleId),
        //        DBCache.DataBase.CreatDbParameter("ArchiveId", archiveId));
        //}


        //public static int SubmitReview(string id)
        //{
        //    var sql = @"update ArchiveSettleInfo set Status = 1 where Id = :Id ";
        //    return DBCache.DataBase.ExecuteNonQuery(
        //        sql,
        //        DBCache.DataBase.CreatDbParameter("Id", id));
        //}

        //#region 审核通过，驳回
        //public static int PassReview(ArchiveSettleInfo data)
        //{
        //    return DBCache.DataBase.ExecuteNonQuery((cmd) =>
        //    {
        //        var ret = 0;
        //        ret = SetInfoPass(cmd, data);

        //        return ChangeArchiveStatus(cmd, data) + ret;
        //    });
        //}

        //private static int SetInfoPass(DbCommand cmd, ArchiveSettleInfo data)
        //{
        //    var sql = @"update ArchiveSettleInfo set Status = 2 where Id = :Id ";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", data.Id));

        //    return cmd.ExecuteNonQuery();
        //}

        //private static int ChangeArchiveStatus(DbCommand cmd, ArchiveSettleInfo data)
        //{
        //    var sql = @"update ArchiveInfo Set Status = :Status, LastEditDate = :LastEditDate
        //                where Id in (select ArchiveId from ArchiveSettleDetail where PId = :PId) ";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", ArchiveStatusType.已结清.GetHashCode()));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("LastEditDate", DateTime.Now));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("PId", data.Id));

        //    return cmd.ExecuteNonQuery();
        //}


        //public static int RollBack(string id)
        //{
        //    var sql = @"update ArchiveSettleInfo set Status = 0 where Id = :Id ";
        //    return DBCache.DataBase.ExecuteNonQuery(
        //        sql,
        //        DBCache.DataBase.CreatDbParameter("Id", id));
        //}
        //#endregion

    }
}
