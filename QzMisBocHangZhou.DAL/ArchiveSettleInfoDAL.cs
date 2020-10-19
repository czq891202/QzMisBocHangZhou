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
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact,ai.Status
                        from ArchiveInfo ai left join OrgInfo o on ai.OrgId = o.Id 
                        where (ai.Status = 1 or ai.Status = 5 or ai.Status = 25 or ai.Status = 23) and ai.Id not in (select ArchiveId from ArchiveSettleInfo where ArchiveId is not null and STATUS <> 3) ";

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
                sql += @" and ast.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId) ";
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
                sql += @" and ast.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId) ";
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

        public static List<ArchiveSettleInfo> GetPreOut()
        {
            var pars = new List<DbParameter>();

            var sql = @"select ast.*, ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower as LoanBorrower,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        From ArchiveSettleInfo ast left join OrgInfo o on ast.OrgId = o.Id 
                        Left join ArchiveInfo ai on ast.ArchiveId = ai.Id
                        where ast.Status = 1 ";

            return DBCache.DataBase.ExecuteEntityList<ArchiveSettleInfo>(sql);
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
        public static int RollBack(ArchiveSettleInfo data)
        {
            return DBCache.DataBase.ExecuteNonQuery((cmd) =>
            {
                var ret = 0;
                ret = SetInfoPass(cmd, data, 3);

                return ArchiveInfoDAL.ChangeArchiveStatus(data.ArchiveId, ArchiveStatusType.结清驳回) + ret;
            });
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

                return ArchiveInfoDAL.ChangeArchiveStatus(data.ArchiveId, arcType) + ret;
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

        public static int SettleOut(string id)
        {
            var sql = @"update ArchiveSettleInfo set Status = 2 where Id = :Id ";
            return DBCache.DataBase.ExecuteNonQuery(
                sql,
                DBCache.DataBase.CreatDbParameter("Id", id));
        }
    }
}
