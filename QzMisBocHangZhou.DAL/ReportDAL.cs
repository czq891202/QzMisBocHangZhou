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
        public static List<ArchiveBorrowInfo> GetBorrowTimeOut(string orgId)
        {
            var pars = new List<DbParameter>();

            var sql = @"select ab.*, ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower as LoanBorrower,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        From ArchiveBorrowInfo ab left join OrgInfo o on ab.OrgId = o.Id 
                        Left join ArchiveInfo ai on ab.ArchiveId = ai.Id
                        where ab.Status = 1 and sysdate > ab.PreReturnDate ";

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and ab.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior ParentId = Id) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            sql += " order by OrgCode, ai.QuotaNo, ai.LoanAccount";

            return DBCache.DataBase.ExecuteEntityList<ArchiveBorrowInfo>(sql, pars.ToArray());
        }


        public static List<ArchiveTransferInfo> GetTransferTimeOut(string orgId, int day)
        {
            var pars = new List<DbParameter>();

            var sql = @"select ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower, ai.Id,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        from ArchiveInfo ai left join OrgInfo o on ai.OrgId = o.Id 
                        where ai.Status = 0  and LoanReleaseDate is not null 
                        and (trunc(sysdate) - trunc(to_date(LoanReleaseDate,'yyyyMMdd'))) > :OutDay";

            pars.Add(DBCache.DataBase.CreatDbParameter("OutDay", day));

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior ParentId = Id) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            sql += " order by OrgCode, ai.LoanReleaseDate, ai.QuotaNo, ai.LoanAccount";

            return DBCache.DataBase.ExecuteEntityList<ArchiveTransferInfo>(sql, pars.ToArray());
            
        }


        public static List<ArchiveSettleInfo> GetSettleTimeOut(string orgId, int day)
        {
            var pars = new List<DbParameter>();

            var sql = @"select ast.*, ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower as LoanBorrower,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        From ArchiveSettleInfo ast left join OrgInfo o on ast.OrgId = o.Id 
                        Left join ArchiveInfo ai on ast.ArchiveId = ai.Id
                        where ast.Status = 1 and (trunc(sysdate) - trunc(SettleDate)) > :OutDay ";

            pars.Add(DBCache.DataBase.CreatDbParameter("OutDay", day));

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and ast.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior ParentId = Id) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            sql += " order by OrgCode, ast.SettleDate, ai.QuotaNo, ai.LoanAccount";

            var rCount = DBCache.DataBase.GetRecordCount(sql, pars.ToArray());
            return DBCache.DataBase.ExecuteEntityList<ArchiveSettleInfo>(sql, pars.ToArray());
        }


        //#region 【借阅】
        //public static List<ReportBorrow> GetByYearMonth(DateTime date)
        //{
        //    var sql = @"select o.id as OrgId, o.code as OrgCode, o.name as OrgName, o.contact as OrgContact, rep.total, rep.borrower from OrgInfo o inner join 
        //                (select b.orgid, b.borrower, count(1) as Total from archiveborrowdetails bd  left join archiveborrowinfo b on bd.pid = b.id 
        //                where (sysdate > bd.prereturndate or bd.realreturndate > bd.prereturndate)
        //                    and to_char(prereturndate,'yyyy-mm') = :TimeOut 
        //                group by b.orgid, b.borrower) rep on o.id = rep.orgid";

        //    return DBCache.DataBase.ExecuteEntityList<ReportBorrow>(sql, DBCache.DataBase.CreatDbParameter("TimeOut", date.ToString("yyyy-MM")));
        //}


        //public static List<ReportBorrow> GetByDay(DateTime date)
        //{
        //    var sql = @"select o.id as OrgId, o.code as OrgCode, o.name as OrgName, o.contact as OrgContact, rep.total, rep.borrower from OrgInfo o inner join 
        //                (select b.orgid, b.borrower, count(1) as Total from archiveborrowdetails bd  left join archiveborrowinfo b on bd.pid = b.id 
        //                where (sysdate > bd.prereturndate or bd.realreturndate > bd.prereturndate)
        //                    and to_char(prereturndate,'yyyy-mm-dd') = :TimeOut 
        //                group by b.orgid, b.borrower) rep on o.id = rep.orgid";

        //    return DBCache.DataBase.ExecuteEntityList<ReportBorrow>(sql, DBCache.DataBase.CreatDbParameter("TimeOut", date.ToString("yyyy-MM-dd")));
        //}

        //#endregion

        //public static List<ReportArchive> GetArchiveTotal()
        //{
        //    var sql = @"select a.status, count(1) as Total from Archiveinfo a group by a.status";

        //    return DBCache.DataBase.ExecuteEntityList<ReportArchive>(sql);
        //}
    }
}
