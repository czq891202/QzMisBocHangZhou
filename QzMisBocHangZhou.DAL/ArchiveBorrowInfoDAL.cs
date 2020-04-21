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
                sql += @" and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId) ";
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
                sql += @" and ab.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId) ";
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
                sql += @" and ab.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId) ";
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
                sql += @" and ab.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId) ";
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
                sql += @" and ab.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId) ";
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
    }
}
