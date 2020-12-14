using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.DAL
{
    public class ReportDAL
    {
        //借阅超时
        public static List<ArchiveBorrowInfo> GetBorrowTimeOut(string orgId, string guaranteeType = "", string keyWords = "")
        {
            var pars = new List<DbParameter>();

            var sql = @"select ab.*, ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower as LoanBorrower,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        From ArchiveBorrowInfo ab left join OrgInfo o on ab.OrgId = o.Id 
                        Left join ArchiveInfo ai on ab.ArchiveId = ai.Id
                        where ab.Status in(2,3) and sysdate > ab.PreReturnDate ";

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and ab.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            if (!string.IsNullOrWhiteSpace(guaranteeType))
            {
                sql += @" and ai.GuaranteeType = :GuaranteeType ";
                pars.Add(DBCache.DataBase.CreatDbParameter("GuaranteeType", guaranteeType));
            }

            if (!string.IsNullOrWhiteSpace(keyWords))
            {
                sql += @" and ai.ProductCode like :ProductCode ";
                pars.Add(DBCache.DataBase.CreatDbParameter("ProductCode", keyWords));
            }

            sql += " order by OrgCode, ai.QuotaNo, ai.LoanAccount";

            return DBCache.DataBase.ExecuteEntityList<ArchiveBorrowInfo>(sql, pars.ToArray());
        }
        //移交超时
        public static List<ArchiveTransferInfo> GetTransferTimeOut(string orgId, int day, string guaranteeType = "", string keyWords = "")
        {
            var pars = new List<DbParameter>();

            var sql = @"select ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower, ai.Id,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        from ArchiveInfo ai left join OrgInfo o on ai.OrgId = o.Id 
                        where NVL(ai.Status,0) = 0 and LoanReleaseDate is not null 
                        and (trunc(sysdate) - trunc(to_date(LoanReleaseDate,'yyyy-MM-dd'))) > :OutDay";

            pars.Add(DBCache.DataBase.CreatDbParameter("OutDay", day));

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            if (!string.IsNullOrWhiteSpace(guaranteeType))
            {
                sql += @" and ai.GuaranteeType = :GuaranteeType ";
                pars.Add(DBCache.DataBase.CreatDbParameter("GuaranteeType", guaranteeType));
            }

            if (!string.IsNullOrWhiteSpace(keyWords))
            {
                sql += @" and ai.ProductCode like :ProductCode ";
                pars.Add(DBCache.DataBase.CreatDbParameter("ProductCode", keyWords));
            }

            sql += " order by OrgCode, ai.LoanReleaseDate, ai.QuotaNo, ai.LoanAccount";

            return DBCache.DataBase.ExecuteEntityList<ArchiveTransferInfo>(sql, pars.ToArray());            
        }
        //结清超时
        public static List<ArchiveSettleInfo> GetSettleTimeOut(string orgId, int day, string guaranteeType = "", string keyWords = "")
        {
            var pars = new List<DbParameter>();

            var sql = @"select ast.*, ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower as LoanBorrower,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        From ArchiveSettleInfo ast left join OrgInfo o on ast.OrgId = o.Id 
                        Left join ArchiveInfo ai on ast.ArchiveId = ai.Id
                        where ast.Status = 0 and (trunc(sysdate) - trunc(SettleDate)) > :OutDay ";

            pars.Add(DBCache.DataBase.CreatDbParameter("OutDay", day));

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and ast.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            if (!string.IsNullOrWhiteSpace(guaranteeType))
            {
                sql += @" and ai.GuaranteeType = :GuaranteeType ";
                pars.Add(DBCache.DataBase.CreatDbParameter("GuaranteeType", guaranteeType));
            }

            if (!string.IsNullOrWhiteSpace(keyWords))
            {
                sql += @" and ai.ProductCode like :ProductCode ";
                pars.Add(DBCache.DataBase.CreatDbParameter("ProductCode", keyWords));
            }

            sql += " order by OrgCode, ast.SettleDate, ai.QuotaNo, ai.LoanAccount";

            var rCount = DBCache.DataBase.GetRecordCount(sql, pars.ToArray());
            return DBCache.DataBase.ExecuteEntityList<ArchiveSettleInfo>(sql, pars.ToArray());
        }
        //档案追溯
        public static PagingResult<ArchiveInfoReport> GetArchiveInfoTime(int page, int limit, string orgId, string guaranteeType, string status, string keyWords)
        {
            var pars = new List<DbParameter>();

            var sql = @"select a.*, o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContatc, b.Handover, b.Receiver, b.TransferDate, b.Status as TransStatus, c.SettlePeople, c.SettleDate, c.Status as SettleStatus, c.UsedBy as  SettleUsedBy, d.Borrower as BorrowPeople, d.BorrowDate, d.UsedBy as BorrowUsedBy, d.Status as BorrowStatus, d.PreReturnDate from ArchiveInfo a left join ArchiveTransferInfo b on a.Id = b.ArchiveId left join ArchiveSettleInfo c on a.Id = c.ArchiveId left join ArchiveBorrowInfo d on a.Id = d.ArchiveId left join OrgInfo o on a.OrgId = o.Id where 1 = 1 ";

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and o.Id in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            if (!string.IsNullOrWhiteSpace(guaranteeType))
            {
                sql += @" and a.GuaranteeType = :GuaranteeType ";
                pars.Add(DBCache.DataBase.CreatDbParameter("GuaranteeType", guaranteeType));
            }

            if (!string.IsNullOrEmpty(status))
            {
                sql += @" and a.Status in (select column_value from table (split (:Status))) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("Status", status));
            }

            if (!string.IsNullOrWhiteSpace(keyWords))
            {
                sql += @" and ( a.ProductCode like :KeyWords or a.Borrower like :KeyWords)";
                pars.Add(DBCache.DataBase.CreatDbParameter("KeyWords", keyWords));
            }

            sql += " order by a.Id, d.BorrowDate asc ";

            var rCount = DBCache.DataBase.GetRecordCount(sql, pars.ToArray());
            var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveInfoReport>(page, limit, sql, pars.ToArray());

            return new PagingResult<ArchiveInfoReport>() { Count = rCount, Result = data };
        }
    }
}
