using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace QzMisBocHangZhou.DAL
{
    public class ArchiveTransferInfoDAL
    {
        public static PagingResult<ArchiveTransferInfo> GetPreTransfer(int page, int limit, string orgId, string keyWords)
        {
            var pars = new List<DbParameter>();

            var sql = @"select ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower, ai.Id,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact, ai.status 
                        from ArchiveInfo ai left join OrgInfo o on ai.OrgId = o.Id 
                        where (ai.Status = 0 or ai.Status = 22) and ai.Id not in (select ArchiveId from ArchiveTransferInfo where ArchiveId is not null and status <> 2) ";

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            if (!string.IsNullOrWhiteSpace(keyWords))
            {
                sql += @" and (LoanAccount like :KeyWords
                        or QuotaNo like :KeyWords 
                        or Borrower like :KeyWords 
                        or CustomerNo like :keywords)";

                pars.Add(DBCache.DataBase.CreatDbParameter("KeyWords", $"%{keyWords.Trim()}%"));
            }

            sql += " order by OrgCode, ai.QuotaNo, ai.LoanAccount";


            var rCount = DBCache.DataBase.GetRecordCount(sql, pars.ToArray());
            var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveTransferInfo>(page, limit, sql, pars.ToArray());

            return new PagingResult<ArchiveTransferInfo>() { Count = rCount, Result = data };
        }

        /// <summary>
        /// 读取移交待审核档案信息
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public static List<InventoryDetail> GetInventoryArchiveList(string orgId)
        {
            var sql = $"select tpa.Id as ArchiveId, tpa.LabelCode from Archiveinfo tpa LEFT JOIN ArchiveTransferInfo ati on tpa.ID = ati.archiveid  where ati.status = 0 ";

            var pars = new List<DbParameter>();
            if (!orgId.Equals(OrgInfo.RootId, StringComparison.OrdinalIgnoreCase))
            {
                sql += @" and tpa.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            sql += " order by tpa.CREATEDATE, tpa.ORGID desc ";
            return DBCache.DataBase.ExecuteEntityList<InventoryDetail>(sql, pars.ToArray());
        }

        public static PagingResult<ArchiveTransferInfo> GetPreReview(int page, int limit, string orgId, string keyWords)
        {
            var pars = new List<DbParameter>();

            var sql = @"select at.*, ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact,
                        nvl(GUARANTEETYPE, '0') as GuaranteType
                        From ArchiveTransferInfo at left join OrgInfo o on at.OrgId = o.Id 
                        Left join ArchiveInfo ai on at.ArchiveId = ai.Id
                        where at.Status = 0 ";

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and at.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId) ";
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
            var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveTransferInfo>(page, limit, sql, pars.ToArray());

            return new PagingResult<ArchiveTransferInfo>() { Count = rCount, Result = data };
        }
        /// <summary>
        /// 导出待审批
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public static List<ArchiveTransferInfo> ExportTransfer(string orgId)
        {
            var pars = new List<DbParameter>();

            var sql = @"select at.*, ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        From ArchiveTransferInfo at left join OrgInfo o on at.OrgId = o.Id 
                        Left join ArchiveInfo ai on at.ArchiveId = ai.Id
                        where at.Status = 0 ";

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and at.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            } 

            sql += " order by OrgCode, ai.QuotaNo, ai.LoanAccount";

            return DBCache.DataBase.ExecuteEntityList<ArchiveTransferInfo>(sql, pars.ToArray());
        }
        /// <summary>
        /// 导出可移交的
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public static List<ArchiveTransferInfo> ExportTransferList(string orgId, string keywords)
        {
            var pars = new List<DbParameter>();

            var sql = @"select ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower, ai.Id,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact, ai.status 
                        from ArchiveInfo ai left join OrgInfo o on ai.OrgId = o.Id 
                        where (ai.Status = 0 or ai.Status = 22) and ai.Id not in (select ArchiveId from ArchiveTransferInfo where ArchiveId is not null and status <> 2) ";

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            if (!string.IsNullOrWhiteSpace(keywords))
            {
                sql += @" and (LoanAccount like :KeyWords
                        or QuotaNo like :KeyWords 
                        or Borrower like :KeyWords 
                        or CustomerNo like :keywords)";

                pars.Add(DBCache.DataBase.CreatDbParameter("KeyWords", $"%{keywords.Trim()}%"));
            }

            sql += " order by OrgCode, ai.QuotaNo, ai.LoanAccount";

            return DBCache.DataBase.ExecuteEntityList<ArchiveTransferInfo>(sql, pars.ToArray());
        }

        public static ArchiveTransferInfo Get(string id)
        {
            var sql = "select * from ArchiveTransferInfo where Id = :Id";

            return DBCache.DataBase.ExecuteEntity<ArchiveTransferInfo>(
                sql,
                DBCache.DataBase.CreatDbParameter("Id", id));
        }


        /// <summary>
        /// 提交审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int SubmitReview(ArchiveInfo arcData, ArchiveTransferInfo transferData)
        {
            return DBCache.DataBase.ExecuteNonQuery((cmd) =>
            {
                var ret = 0;
                ret = ArchiveInfoDAL.Update(cmd, arcData);
                return ret + AddTransferInfo(cmd, transferData);
            });
        }
        /// <summary>
        /// 判断电子标签是否使用
        /// </summary>
        /// <param name="labelcode"></param>
        /// <returns></returns>
        public static int CheckLabelCode(string labelcode,string customerno)
        {
            var sql = "SELECT * FROM ArchiveInfo WHERE LabelCode = :LabelCode and CustomerNo <> :CustomerNo";
            return DBCache.DataBase.ExecuteNonQuery(sql,
                DBCache.DataBase.CreatDbParameter("CustomerNo", customerno),
                DBCache.DataBase.CreatDbParameter("LabelCode", labelcode));
        }

        private static int AddTransferInfo(DbCommand cmd, ArchiveTransferInfo transferData)
        {
            var sql = "insert into ArchiveTransferInfo (Id, OrgId, Handover, Receiver, Status, TransferDate, ArchiveId) values(:Id, :OrgId, :Handover, :Receiver, :Status, :TransferDate, :ArchiveId)";
            cmd.CommandText = sql;

            cmd.Parameters.Clear();
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", transferData.Id));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("OrgId", transferData.OrgId));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Handover", transferData.Handover));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Receiver", transferData.Receiver));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", transferData.Status));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("TransferDate", transferData.TransferDate));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("ArchiveId", transferData.ArchiveId));

            return cmd.ExecuteNonQuery();
        }


        /// <summary>
        /// 撤回/驳回
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int RollBack(ArchiveTransferInfo data)
        {
            return DBCache.DataBase.ExecuteNonQuery((cmd) =>
            {
                var ret = 0;
                ret = SetInfoPass(cmd, data, "2");

                return ArchiveInfoDAL.ChangeArchiveStatus(data.ArchiveId, ArchiveStatusType.移交驳回) + ret;
            });
        }


        public static int PassReview(ArchiveTransferInfo data)
        {
            return DBCache.DataBase.ExecuteNonQuery((cmd) =>
            {
                var ret = 0;
                ret = SetInfoPass(cmd, data, "1");

                return ArchiveInfoDAL.ChangeArchiveStatus(data.ArchiveId, ArchiveStatusType.已入库) + ret;
            });
        }

        private static int SetInfoPass(DbCommand cmd, ArchiveTransferInfo data, string status)
        {
            var sql = @"update ArchiveTransferInfo set Status = :Status, Receiver = :Receiver where Id = :Id ";
            cmd.CommandText = sql;

            cmd.Parameters.Clear();
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", status));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Receiver", data.Receiver));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", data.Id));

            return cmd.ExecuteNonQuery();
        }
    }
}
