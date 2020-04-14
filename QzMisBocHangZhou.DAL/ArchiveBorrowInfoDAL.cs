using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.DAL
{
    public class ArchiveBorrowInfoDAL
    {
        public static ArchiveBorrowInfo Get(string id)
        {
            var sql = @"select * from ArchiveBorrowInfo where Id = :Id";

            return DBCache.DataBase.ExecuteEntity<ArchiveBorrowInfo>(sql, 
                DBCache.DataBase.CreatDbParameter("Id", id));
        }

        public static PagingResult<ArchiveBorrowInfo> GetPreBorrow(int page, int limit, string orgId, string keyWords)
        {
            var pars = new List<DbParameter>();

            var sql = @"select ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower as LoanBorrower, ai.Id,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        from ArchiveInfo ai left join OrgInfo o on ai.OrgId = o.Id 
                        where ai.Status = 1  and ai.Id not in (select ArchiveId from ArchiveBorrowInfo where Status <> 2 and ArchiveId is not null) ";

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
            var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveBorrowInfo>(page, limit, sql, pars.ToArray());

            return new PagingResult<ArchiveBorrowInfo>() { Count = rCount, Result = data };
        }

        public static PagingResult<ArchiveBorrowInfo> GetPreReview(int page, int limit, string orgId, string keyWords)
        {
            var pars = new List<DbParameter>();

            var sql = @"select ab.*, ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower as LoanBorrower,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        From ArchiveBorrowInfo ab left join OrgInfo o on ab.OrgId = o.Id 
                        Left join ArchiveInfo ai on ab.ArchiveId = ai.Id
                        where ab.Status = 0 ";

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and ab.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) ";
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
            var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveBorrowInfo>(page, limit, sql, pars.ToArray());

            return new PagingResult<ArchiveBorrowInfo>() { Count = rCount, Result = data };
        }

        public static List<ArchiveBorrowInfo> GetExcelData(string orgId)
        {
            var pars = new List<DbParameter>();

            var sql = @"select ab.*, ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower as LoanBorrower,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        From ArchiveBorrowInfo ab left join OrgInfo o on ab.OrgId = o.Id 
                        Left join ArchiveInfo ai on ab.ArchiveId = ai.Id
                        where ab.Status = 0 ";

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and ab.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            sql += " order by OrgCode, ai.QuotaNo, ai.LoanAccount";

            return DBCache.DataBase.ExecuteEntityList<ArchiveBorrowInfo>(sql, pars.ToArray());
        }

        public static int SubmitReview(ArchiveBorrowInfo data)
        {
            var sql = @"insert into ArchiveBorrowInfo (Id, OrgId, Borrower, BorrowDate, Status, ArchiveId, PreReturnDate, UsedBy) 
                        values(:Id, :OrgId, :Borrower, :BorrowDate, :Status, :ArchiveId, :PreReturnDate, :UsedBy)";
            return DBCache.DataBase.ExecuteNonQuery(sql,
                DBCache.DataBase.CreatDbParameter("Id", data.Id),
                DBCache.DataBase.CreatDbParameter("OrgId", data.OrgId),
                DBCache.DataBase.CreatDbParameter("Borrower", data.Borrower),
                DBCache.DataBase.CreatDbParameter("BorrowDate", data.BorrowDate),
                DBCache.DataBase.CreatDbParameter("Status", data.Status),
                DBCache.DataBase.CreatDbParameter("ArchiveId", data.ArchiveId),
                DBCache.DataBase.CreatDbParameter("PreReturnDate", data.PreReturnDate),
                DBCache.DataBase.CreatDbParameter("UsedBy", data.UsedBy));
        }

        /// <summary>
        /// 撤回/驳回
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int RollBack(string id)
        {
            var sql = @"delete ArchiveBorrowInfo where Id = :Id ";
            return DBCache.DataBase.ExecuteNonQuery(
                sql,
                DBCache.DataBase.CreatDbParameter("Id", id));
        }

        public static int PassReview(ArchiveBorrowInfo data)
        {
            return DBCache.DataBase.ExecuteNonQuery((cmd) =>
            {
                var ret = 0;
                ret = SetInfoPass(cmd, data, 1);

                ret += ChangeArchiveStatus(cmd, data, ArchiveStatusType.借阅出库);                

                return ret;
            });
        }

        public static int Returned(ArchiveBorrowInfo data)
        {
            return DBCache.DataBase.ExecuteNonQuery((cmd) =>
            {
                var ret = 0;
                ret = SetInfoPass(cmd, data, 2);

                ret += ChangeArchiveStatus(cmd, data, ArchiveStatusType.已入库);

                ret += SetReturnDate(cmd, data);

                return ret;
            });
        }

        private static int SetInfoPass(DbCommand cmd, ArchiveBorrowInfo data, int status)
        {
            var sql = @"update ArchiveBorrowInfo set Status = :Status where Id = :Id ";
            cmd.CommandText = sql;

            cmd.Parameters.Clear();
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", data.Id));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", status));

            return cmd.ExecuteNonQuery();
        }

        private static int ChangeArchiveStatus(DbCommand cmd, ArchiveBorrowInfo data, ArchiveStatusType status)
        {

            var sql = @"update ArchiveInfo Set Status = :Status
                        where Id = :Id";
            cmd.CommandText = sql;

            cmd.Parameters.Clear();
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", data.ArchiveId));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", status.GetHashCode()));
            return cmd.ExecuteNonQuery();
        }

        private static int SetReturnDate(DbCommand cmd, ArchiveBorrowInfo data)
        {
            var sql = @"update ArchiveBorrowInfo set RealReturnDate = :RealReturnDate where Id = :Id ";
            cmd.CommandText = sql;

            cmd.Parameters.Clear();
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", data.Id));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("RealReturnDate", DateTime.Now));

            return cmd.ExecuteNonQuery();
        }

        public static int ChangeIn(ArchiveInfo arcData, ArchiveBorrowInfo borrowData)
        {
            return DBCache.DataBase.ExecuteNonQuery((cmd) =>
            {
                var ret = 0;
                ret += ArchiveInfoDAL.Update(cmd, arcData);
                ret += SetInfoPass(cmd, borrowData, 2);
                ret += ChangeArchiveStatus(cmd, borrowData, ArchiveStatusType.已入库);
                ret += SetReturnDate(cmd, borrowData);
                
                return ret;
            });
        }

        #region 【归还相关审批】
        /// <summary>
        /// 获取待归还列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="orgId"></param>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public static PagingResult<ArchiveBorrowInfo> GetPreGiveBack(int page, int limit, string orgId, string keyWords)
        {
            var pars = new List<DbParameter>();

            var sql = @"select ab.*, ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower as LoanBorrower,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        From ArchiveBorrowInfo ab left join OrgInfo o on ab.OrgId = o.Id 
                        Left join ArchiveInfo ai on ab.ArchiveId = ai.Id
                        where ab.Status = 1 ";

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and ab.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) ";
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
            var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveBorrowInfo>(page, limit, sql, pars.ToArray());

            return new PagingResult<ArchiveBorrowInfo>() { Count = rCount, Result = data };
        }

        /// <summary>
        /// 提交归还审批
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int SubmitGiveBack(ArchiveBorrowInfo data)
        {
            var sql = @"update ArchiveBorrowInfo set Status = :Status,PreReturnDate = :PreReturnDate where Id = :Id";
            return DBCache.DataBase.ExecuteNonQuery(sql,
                DBCache.DataBase.CreatDbParameter("Id", data.Id),
                DBCache.DataBase.CreatDbParameter("Status", data.Status),
                DBCache.DataBase.CreatDbParameter("PreReturnDate", data.PreReturnDate));
        }
        /// <summary>
        /// 获取归还待审批列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="orgId"></param>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public static PagingResult<ArchiveBorrowInfo> GetGiveBackReview(int page, int limit, string orgId, string keyWords)
        {
            var pars = new List<DbParameter>();

            var sql = @"select ab.*, ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower as LoanBorrower,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        From ArchiveBorrowInfo ab left join OrgInfo o on ab.OrgId = o.Id 
                        Left join ArchiveInfo ai on ab.ArchiveId = ai.Id
                        where ab.Status = 3 ";

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and ab.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) ";
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
            var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveBorrowInfo>(page, limit, sql, pars.ToArray());

            return new PagingResult<ArchiveBorrowInfo>() { Count = rCount, Result = data };
        }
        /// <summary>
        /// 归还待审批撤回/驳回
        /// </summary>
        /// <param name="id">借阅id</param>
        /// <returns></returns>
        public static int GiveBackRollBack(string id)
        {
            var sql = @"update ArchiveBorrowInfo set Status = 1 where Id = :Id ";
            return DBCache.DataBase.ExecuteNonQuery(
                sql,
                DBCache.DataBase.CreatDbParameter("Id", id));
        }
        /// <summary>
        /// 读取归还待审核档案信息
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public static List<InventoryDetail> GetInventoryArchiveList(string orgId)
        {
            var sql = $"select tpa.Id as ArchiveId, tpa.LabelCode from Archiveinfo tpa left join ArchiveBorrowInfo abi on tpa.Id = abi.archiveid  where tpa.STATUS = {ArchiveStatusType.借阅出库.GetHashCode()} and abi.status = 3 ";

            var pars = new List<DbParameter>();
            if (!orgId.Equals(OrgInfo.RootId, StringComparison.OrdinalIgnoreCase))
            {
                sql += @" and tpa.OrgId = :OrgId ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            sql += " order by tpa.CREATEDATE, tpa.ORGID desc ";
            return DBCache.DataBase.ExecuteEntityList<InventoryDetail>(sql, pars.ToArray());
        }
        #endregion


        //#region Get Info
        //public static ArchiveBorrowInfo Get(string id)
        //{
        //    var sql = @"select a.*, o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact 
        //                from ArchiveBorrowInfo a left join OrgInfo o on a.OrgId = o.Id
        //                where a.Id = :Id ";

        //    return DBCache.DataBase.ExecuteEntity<ArchiveBorrowInfo>(
        //        sql,
        //        DBCache.DataBase.CreatDbParameter("Id", id));
        //}

        //public static PagingResult<ArchiveBorrowInfo> Get(int page, int limit)
        //{
        //    var sql = @"select a.*, o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact  
        //                from ArchiveBorrowInfo a left join OrgInfo o on a.OrgId = o.Id 
        //                order by a.Status, OrgCode, a.BorrowDate desc";

        //    var rCount = DBCache.DataBase.GetRecordCount(sql);
        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveBorrowInfo>(page, limit, sql);

        //    return new PagingResult<ArchiveBorrowInfo>() { Count = rCount, Result = data };

        //}


        //public static PagingResult<ArchiveBorrowInfo> GetByOrg(int page, int limit, string orgId)
        //{
        //    var sql = @"select a.*, o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact  
        //                from ArchiveBorrowInfo a left join OrgInfo o on a.OrgId = o.Id
        //                where OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) 
        //                order by a.Status, OrgCode, a.BorrowDate desc";

        //    var rCount = DBCache.DataBase.GetRecordCount(sql, DBCache.DataBase.CreatDbParameter("OrgId", orgId));
        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveBorrowInfo>(page, limit, sql,
        //        DBCache.DataBase.CreatDbParameter("OrgId", orgId));

        //    return new PagingResult<ArchiveBorrowInfo>() { Count = rCount, Result = data };
        //}


        //public static PagingResult<ArchiveBorrowInfo> GetApprovaBorrowList(int page, int limit)
        //{
        //    var sql = @"select a.*, o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact  
        //                from ArchiveBorrowInfo a left join OrgInfo o on a.OrgId = o.Id 
        //                where a.Status <> 0
        //                order by a.Status, OrgCode, a.BorrowDate desc";

        //    var rCount = DBCache.DataBase.GetRecordCount(sql);
        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveBorrowInfo>(page, limit, sql);

        //    return new PagingResult<ArchiveBorrowInfo>() { Count = rCount, Result = data };

        //}


        //public static PagingResult<ArchiveBorrowInfo> GetApprovaBorrowList(int page, int limit, string orgId)
        //{
        //    var sql = @"select a.*, o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact  
        //                from ArchiveBorrowInfo a left join OrgInfo o on a.OrgId = o.Id
        //                where OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) 
        //                and a.Status <> 0
        //                order by a.Status, OrgCode, a.BorrowDate desc";

        //    var rCount = DBCache.DataBase.GetRecordCount(sql, DBCache.DataBase.CreatDbParameter("OrgId", orgId));
        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveBorrowInfo>(page, limit, sql,
        //        DBCache.DataBase.CreatDbParameter("OrgId", orgId));

        //    return new PagingResult<ArchiveBorrowInfo>() { Count = rCount, Result = data };
        //}

        //#endregion


        //public static PagingResult<SelectArchiveModel> GetArchiveList(int page, int limit, string borrowId)
        //{
        //    var sql = @"select a.*, b.IsChecked, o.name as OrgName, o.code as OrgCode from ArchiveInfo a 
        //                left join (select archiveid, 1 as IsChecked from ArchiveBorrowDetails where pid = :borrowId) b on a.id = b.archiveid
        //                left join orginfo o on a.orgid = o.id
        //                where a.id not in(
        //                        select atd.archiveid from ArchiveBorrowDetails atd left join ArchiveBorrowInfo at on atd.pid = at.id
        //                        where at.status <> 0 and at.status <>3 and atd.IsReturned = 0)
        //                    and (a.status = 1) and (b.IsChecked is null or b.IsChecked <> 1)
        //                order by  b.IsChecked, a.orgid, a.quotano, a.loanaccount";

        //    var rCount = DBCache.DataBase.GetRecordCount(sql, DBCache.DataBase.CreatDbParameter("borrowId", borrowId));
        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<SelectArchiveModel>(page, limit, sql, DBCache.DataBase.CreatDbParameter("borrowId", borrowId));

        //    return new PagingResult<SelectArchiveModel>() { Count = rCount, Result = data };
        //}


        //public static PagingResult<SelectArchiveModel> GetArchiveList(int page, int limit, string orgId, string borrowId)
        //{
        //    var sql = @"select a.*, b.IsChecked, o.name as OrgName, o.code as OrgCode from ArchiveInfo a 
        //                left join (select archiveid, 1 as IsChecked from ArchiveBorrowDetails where pid = :borrowId) b on a.id = b.archiveid
        //                left join orginfo o on a.orgid = o.id
        //                where a.id not in(
        //                        select atd.archiveid from ArchiveBorrowDetails atd left join ArchiveBorrowInfo at on atd.pid = at.id 
        //                        where at.status <> 0 and at.status <>3 and atd.IsReturned = 0)
        //                    and (a.status = 1) and (b.IsChecked is null or b.IsChecked <> 1)
        //                    and a.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) 
        //                order by  b.IsChecked, a.orgid, a.quotano, a.loanaccount";


        //    var rCount = DBCache.DataBase.GetRecordCount(sql,
        //        DBCache.DataBase.CreatDbParameter("borrowId", borrowId),
        //        DBCache.DataBase.CreatDbParameter("OrgId", orgId));

        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<SelectArchiveModel>(page, limit, sql,
        //        DBCache.DataBase.CreatDbParameter("borrowId", borrowId),
        //        DBCache.DataBase.CreatDbParameter("OrgId", orgId));

        //    return new PagingResult<SelectArchiveModel>() { Count = rCount, Result = data };
        //}


        //public static ArchiveBorrowDetails GetDetail(string id)
        //{
        //    var sql = "select t.*, a.QuotaNo, a.LoanAccount, a.LabelCode, a.Borrower, a.GuaranteeCrdNo from ArchiveBorrowDetails t left join archiveinfo a on t.archiveid = a.id where t.Id =:Id ";

        //    return DBCache.DataBase.ExecuteEntity<ArchiveBorrowDetails>(
        //        sql,
        //        DBCache.DataBase.CreatDbParameter("Id", id));
        //}

        //public static List<ArchiveBorrowDetails> GetDetails(string pId)
        //{
        //    var sql = "select t.*, a.QuotaNo, a.LoanAccount, a.LabelCode, a.Borrower, a.GuaranteeCrdNo from ArchiveBorrowDetails t left join archiveinfo a on t.archiveid = a.id where t.PId =:PId ";

        //    return DBCache.DataBase.ExecuteEntityList<ArchiveBorrowDetails>(
        //        sql,
        //        DBCache.DataBase.CreatDbParameter("PId", pId));
        //}


        //public static int AddInfo(ArchiveBorrowInfo data)
        //{
        //    var sql = @"insert into ArchiveBorrowInfo (Id, OrgId, Borrower, BorrowDate, ArchiveType, Status) values(:Id, :OrgId, :Borrower, :BorrowDate, :ArchiveType, :Status)";
        //    return DBCache.DataBase.ExecuteNonQuery(sql, 
        //        DBCache.DataBase.CreatDbParameter("Id", data.Id),
        //        DBCache.DataBase.CreatDbParameter("OrgId", data.OrgId),
        //        DBCache.DataBase.CreatDbParameter("Borrower", data.Borrower),
        //        DBCache.DataBase.CreatDbParameter("BorrowDate", data.BorrowDate),
        //        DBCache.DataBase.CreatDbParameter("ArchiveType", data.ArchiveType),
        //        DBCache.DataBase.CreatDbParameter("Status", data.Status));
        //}


        //#region 【更新】
        //public static int SubmitReview(string id)
        //{
        //    var sql = @"update ArchiveBorrowInfo set Status = 1 where Id = :Id ";
        //    return DBCache.DataBase.ExecuteNonQuery(
        //        sql,
        //        DBCache.DataBase.CreatDbParameter("Id", id));
        //}


        //public static int UpdateInfo(ArchiveBorrowInfo info)
        //{
        //    var sql = @"update ArchiveBorrowInfo set OrgId = :OrgId, Borrower = :Borrower, BorrowDate = :BorrowDate, 
        //                    ArchiveType = :ArchiveType, Status = :Status where Id = :Id";
        //    var ret = DBCache.DataBase.ExecuteNonQuery(sql,
        //        DBCache.DataBase.CreatDbParameter("OrgId", info.OrgId),
        //        DBCache.DataBase.CreatDbParameter("Borrower", info.Borrower),
        //        DBCache.DataBase.CreatDbParameter("BorrowDate", info.BorrowDate),
        //        DBCache.DataBase.CreatDbParameter("ArchiveType", info.ArchiveType),
        //        DBCache.DataBase.CreatDbParameter("Status", info.Status),
        //        DBCache.DataBase.CreatDbParameter("Id", info.Id));

        //    if (ret == 0) return 0;

        //    var details = GetUpdateDetails(info.Id);
        //    if (details == null || details.Count == 0) return ret;

        //    ret += UpdatePreReturnDate(details);

        //    return ret;
        //}

        //private static int UpdatePreReturnDate(List<ArchiveBorrowDetails> details)
        //{
        //    var sql = @"update ArchiveBorrowDetails set PreReturnDate = :PreReturnDate where Id = :Id";
        //    return DBCache.DataBase.ExecuteNonQuery((cmd) =>
        //    {
        //        cmd.CommandText = sql;
        //        var ret = 0;
        //        foreach(var item in details)
        //        {
        //            cmd.Parameters.Clear();
        //            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("PreReturnDate", item.PreReturnDate));
        //            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", item.Id));
        //            ret += cmd.ExecuteNonQuery();
        //        }

        //        return ret;
        //    });
        //}

        //private static List<ArchiveBorrowDetails> GetUpdateDetails(string pId)
        //{
        //    var sql = @"select ad.Id, (ai.borrowdate + atime.day) as PreReturnDate from archiveborrowdetails ad
        //                left join ARCHIVEBORROWINFO ai on ad.pid = ai.id
        //                left join ARCHIVEPROCTIME atime on ad.usedby = atime.proctype
        //                where ad.pId = :pId";

        //    return DBCache.DataBase.ExecuteEntityList<ArchiveBorrowDetails>(
        //        sql,
        //        DBCache.DataBase.CreatDbParameter("PId", pId));
        //}

        //#endregion


        //public static int Del(string id)
        //{
        //    return DBCache.DataBase.ExecuteNonQuery((cmd) =>
        //    {
        //        var ret = 0;
        //        ret = DelInfo(cmd, id);

        //        return DelDetail(cmd, id) + ret;
        //    });
        //}


        //public static int DelDetail(string id)
        //{
        //    var sql = "delete from ArchiveBorrowDetails where Id = :Id ";
        //    return DBCache.DataBase.ExecuteNonQuery(sql, DBCache.DataBase.CreatDbParameter("Id", id));
        //}


        //private static int DelInfo(DbCommand cmd, string id)
        //{
        //    var sql = "delete from ArchiveBorrowInfo where Id = :Id and Status = 0 ";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", id));
        //    return cmd.ExecuteNonQuery();
        //}


        //private static int DelDetail(DbCommand cmd, string transferId)
        //{
        //    var sql = "delete from ArchiveBorrowDetails where PId = :PId ";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("PId", transferId));
        //    return cmd.ExecuteNonQuery();
        //}


        //public static int AddDetails(List<ArchiveBorrowDetails> arcList)
        //{
        //    return DBCache.DataBase.ExecuteNonQuery((cmd) =>
        //    {
        //        var ret = 0;
        //        foreach(var item in arcList)
        //        {
        //            ret += AddDetails(cmd, item);

        //        }
        //        return ret;
        //    });
        //}


        //private static int AddDetails(DbCommand cmd, ArchiveBorrowDetails details)
        //{
        //    var sql = "insert into ArchiveBorrowDetails (Id, PId, ArchiveId, UsedBy, PreReturnDate, IsReturned, Status) values(:Id, :PId, :ArchiveId, :UsedBy, :PreReturnDate, 0, 0)";
        //    cmd.CommandText = sql;
        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", details.Id));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("PId", details.PId));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("ArchiveId", details.ArchiveId));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("UsedBy", details.UsedBy));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("PreReturnDate", details.PreReturnDate));

        //    return cmd.ExecuteNonQuery();
        //}


        //#region 审核通过，驳回
        //public static int PassReview(ArchiveBorrowInfo data)
        //{
        //    return DBCache.DataBase.ExecuteNonQuery((cmd) =>
        //    {
        //        var ret = 0;
        //        ret = SetInfoPass(cmd, data);

        //        //变更出库
        //        ret += ChangeArchiveStatus(cmd, ArchiveStatusType.变更出库.GetHashCode(), data.Id, "期转现");

        //        //处置出库
        //        ret += ChangeArchiveStatus(cmd, ArchiveStatusType.处置出库.GetHashCode(), data.Id, "诉讼");
        //        ret += ChangeArchiveStatus(cmd, ArchiveStatusType.处置出库.GetHashCode(), data.Id, "核销");

        //        //借阅出库
        //        ret += ChangeArchiveStatus(cmd, ArchiveStatusType.借阅出库.GetHashCode(), data.Id, "复印");
        //        ret += ChangeArchiveStatus(cmd, ArchiveStatusType.借阅出库.GetHashCode(), data.Id, "审计");
        //        ret += ChangeArchiveStatus(cmd, ArchiveStatusType.借阅出库.GetHashCode(), data.Id, "其他");

        //        return ret;
        //    });
        //}

        //private static int SetInfoPass(DbCommand cmd, ArchiveBorrowInfo data)
        //{
        //    var sql = @"update ArchiveBorrowInfo set Status = 2 where Id = :Id ";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", data.Id));

        //    return cmd.ExecuteNonQuery();
        //}


        ////private static int ChangeArchiveStatus(DbCommand cmd, ArchiveBorrowInfo data)
        ////{
        ////    
        ////    var sql = @"update ArchiveInfo Set Status = 2
        ////                where Id in (select ArchiveId from ArchiveBorrowDetails where PId = :PId) ";
        ////    cmd.CommandText = sql;

        ////    cmd.Parameters.Clear();
        ////    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("PId", data.Id));

        ////    return cmd.ExecuteNonQuery();
        ////}

        //private static int ChangeArchiveStatus(DbCommand cmd, int arcStatus, string pId, string usedBy)
        //{
        //    var sql = @"update ArchiveInfo Set Status = :Status
        //                where Id in (select ArchiveId from ArchiveBorrowDetails where PId = :PId and UsedBy = :UsedBy) ";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", arcStatus));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("PId", pId));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("UsedBy", usedBy));
        //    return cmd.ExecuteNonQuery();
        //}



        //public static int RollBack(string id)
        //{
        //    var sql = @"update ArchiveBorrowInfo set Status = 0 where Id = :Id ";
        //    return DBCache.DataBase.ExecuteNonQuery(
        //        sql,
        //        DBCache.DataBase.CreatDbParameter("Id", id));
        //}
        //#endregion


        //#region 【借阅归还】
        //public static int ReturnArchive(ArchiveBorrowDetails details)
        //{
        //    var isAllReturn = IsAllReturned(details.Id, details.PId);

        //    return DBCache.DataBase.ExecuteNonQuery((cmd) =>
        //    {
        //        var ret = 0;
        //        ret = UpdateReturnInfo(cmd, details);
        //        ret += ChangeArchiveStatusBack(cmd, details.ArchiveId);

        //        if(isAllReturn)
        //        {
        //            ret += ChangeBorrowStatusComplate(cmd, details.PId);
        //        }

        //        return ret;
        //    });
        //}


        //private static bool IsAllReturned(string id, string pId)
        //{
        //    var sql = @"select count(1) from ArchiveBorrowDetails where IsReturned = 0 and PId = :PId and Id <> :Id";
        //    var ret = DBCache.DataBase.ExecuteScalar<int>(sql,
        //        DBCache.DataBase.CreatDbParameter("PId", pId),
        //        DBCache.DataBase.CreatDbParameter("Id", id));
        //    return ret == 0;
        //}


        //private static int UpdateReturnInfo(DbCommand cmd, ArchiveBorrowDetails details)
        //{
        //    var sql = @"update ArchiveBorrowDetails set RealReturnDate = :RealReturnDate, ReturnPepole = :ReturnPepole, 
        //                    Remark = :Remark, IsReturned = 1 where Id = :Id";

        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("RealReturnDate", details.RealReturnDate));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("ReturnPepole", details.ReturnPepole.Trim()));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Remark", details.Remark.Trim()));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", details.Id));

        //    return cmd.ExecuteNonQuery();
        //}


        //private static int ChangeArchiveStatusBack(DbCommand cmd, string archiveId)
        //{
        //    var sql = @"update ArchiveInfo Set Status = :Status where Id = :Id ";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", ArchiveStatusType.已入库.GetHashCode()));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", archiveId));

        //    return cmd.ExecuteNonQuery();
        //}


        //private static int ChangeBorrowStatusComplate(DbCommand cmd, string pId)
        //{
        //    var sql = @"update ArchiveBorrowInfo set Status = 3 where Id = :Id ";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", pId));

        //    return cmd.ExecuteNonQuery();
        //}

        //#endregion

        //public static List<InventoryDetail> GetBorrowInventoryDetails(string pId)
        //{
        //    var sql = @"select t.Id, a.LabelCode from ArchiveBorrowDetails t
        //                left join ArchiveInfo a on t.archiveid = a.id where t.PID = :PID";
        //    return DBCache.DataBase.ExecuteEntityList<InventoryDetail>(sql, DBCache.DataBase.CreatDbParameter("PID", pId));
        //}


        //public static int VerifyData(List<ArchiveBorrowDetails> details)
        //{
        //    if (details == null || details.Count == 0) return 0;
        //    return DBCache.DataBase.ExecuteNonQuery((cmd) =>
        //    {
        //        var ret = 0;
        //        foreach (var item in details)
        //        {
        //            ret += VerifyData(cmd, item);
        //        }

        //        return ret;
        //    });
        //}

        //private static int VerifyData(DbCommand cmd, ArchiveBorrowDetails data)
        //{
        //    cmd.CommandText = "update ArchiveBorrowDetails set Status = :Status where Id = :Id";

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", data.Status.GetHashCode()));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", data.Id));

        //    return cmd.ExecuteNonQuery();
        //}
    }
}
