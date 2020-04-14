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
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        from ArchiveInfo ai left join OrgInfo o on ai.OrgId = o.Id 
                        where ai.Status = 0  and ai.Id not in (select ArchiveId from ArchiveTransferInfo where ArchiveId is not null) ";

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) ";
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
                sql += @" and tpa.OrgId = :OrgId ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            sql += " order by tpa.CREATEDATE, tpa.ORGID desc ";
            return DBCache.DataBase.ExecuteEntityList<InventoryDetail>(sql, pars.ToArray());
        }

        public static PagingResult<ArchiveTransferInfo> GetPreReview(int page, int limit, string orgId, string keyWords)
        {
            var pars = new List<DbParameter>();

            var sql = @"select at.*, ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        From ArchiveTransferInfo at left join OrgInfo o on at.OrgId = o.Id 
                        Left join ArchiveInfo ai on at.ArchiveId = ai.Id
                        where at.Status = 0 ";

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and at.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) ";
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


        public static List<ArchiveTransferInfo> GetExcelData(string orgId)
        {
            var pars = new List<DbParameter>();

            var sql = @"select at.*, ai.LabelCode, ai.LoanAccount, ai.QuotaNo, ai.CustomerNo, ai.Borrower,
                        o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact
                        From ArchiveTransferInfo at left join OrgInfo o on at.OrgId = o.Id 
                        Left join ArchiveInfo ai on at.ArchiveId = ai.Id
                        where at.Status = 0 ";

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and at.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
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
        public static int RollBack(string id)
        {
            var sql = @"delete ArchiveTransferInfo where Id = :Id ";
            return DBCache.DataBase.ExecuteNonQuery(
                sql,
                DBCache.DataBase.CreatDbParameter("Id", id));
        }


        public static int PassReview(ArchiveTransferInfo data)
        {
            return DBCache.DataBase.ExecuteNonQuery((cmd) =>
            {
                var ret = 0;
                ret = SetInfoPass(cmd, data);

                return ChangeArchiveStatus(cmd, data) + ret;
            });
        }

        private static int SetInfoPass(DbCommand cmd, ArchiveTransferInfo data)
        {
            var sql = @"update ArchiveTransferInfo set Status = 1, Receiver = :Receiver where Id = :Id ";
            cmd.CommandText = sql;

            cmd.Parameters.Clear();
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Receiver", data.Receiver));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", data.Id));

            return cmd.ExecuteNonQuery();
        }

        private static int ChangeArchiveStatus(DbCommand cmd, ArchiveTransferInfo data)
        {
            var sql = @"update ArchiveInfo Set Status = :Status, LastEditDate = :LastEditDate
                        where Id = :Id ";
            cmd.CommandText = sql;

            cmd.Parameters.Clear();
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", ArchiveStatusType.已入库.GetHashCode()));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("LastEditDate", DateTime.Now));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", data.ArchiveId));

            return cmd.ExecuteNonQuery();
        }


        #region 【作废】

        //public static PagingResult<ArchiveTransferInfo> GetByOrg(int page, int limit, string orgId)
        //{
        //    var sql = @"select a.*, o.Name as OrgName, o.Code as OrgCode 
        //                from ArchiveTransferInfo a left join OrgInfo o on a.OrgId = o.Id
        //                where OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) 
        //                order by a.Status, OrgCode, a.TransferDate desc";

        //    var rCount = DBCache.DataBase.GetRecordCount(sql, DBCache.DataBase.CreatDbParameter("OrgId", orgId));
        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveTransferInfo>(page, limit, sql,
        //        DBCache.DataBase.CreatDbParameter("OrgId", orgId));

        //    return new PagingResult<ArchiveTransferInfo>() { Count = rCount, Result = data };
        //}


        //public static ArchiveTransferInfo Get(string id)
        //{
        //    var sql = @"select a.*, o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact 
        //                from ArchiveTransferInfo a left join OrgInfo o on a.OrgId = o.Id
        //                where a.Id = :Id order by a.OrgId, a.TransferDate desc";

        //    return DBCache.DataBase.ExecuteEntity<ArchiveTransferInfo>(
        //        sql,
        //        DBCache.DataBase.CreatDbParameter("Id", id));
        //}


        //public static PagingResult<ArchiveTransferInfo> GetApprovaTransferList(int page, int limit)
        //{
        //    var sql = @"select a.*, o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact 
        //                from ArchiveTransferInfo a left join OrgInfo o on a.OrgId = o.Id 
        //                where a.Status <> 0
        //                order by a.Status, OrgCode, a.TransferDate desc";

        //    var rCount = DBCache.DataBase.GetRecordCount(sql);
        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveTransferInfo>(page, limit, sql);

        //    return new PagingResult<ArchiveTransferInfo>() { Count = rCount, Result = data };

        //}


        //public static PagingResult<ArchiveTransferInfo> GetApprovaTransferList(int page, int limit, string orgId)
        //{
        //    var sql = @"select a.*, o.Name as OrgName, o.Code as OrgCode, o.Contact as OrgContact 
        //                from ArchiveTransferInfo a left join OrgInfo o on a.OrgId = o.Id
        //                where OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) 
        //                and a.Status <> 0
        //                order by a.Status, OrgCode, a.TransferDate desc";

        //    var rCount = DBCache.DataBase.GetRecordCount(sql, DBCache.DataBase.CreatDbParameter("OrgId", orgId));
        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveTransferInfo>(page, limit, sql,
        //        DBCache.DataBase.CreatDbParameter("OrgId", orgId));

        //    return new PagingResult<ArchiveTransferInfo>() { Count = rCount, Result = data };
        //}


        //public static List<ArchiveTransferDetails> GetDetails(string pId)
        //{
        //    var sql = "select t.*, a.QuotaNo, a.LoanAccount, a.LabelCode, a.Borrower from archivetransferdetails t left join archiveinfo a on t.archiveid = a.id where t.PId =:PId ";

        //    return DBCache.DataBase.ExecuteEntityList<ArchiveTransferDetails>(
        //        sql,
        //        DBCache.DataBase.CreatDbParameter("PId", pId));
        //}


        //public static ArchiveTransferDetails GetDetails(string pId, string archiveId)
        //{
        //    var sql = "select t.*, a.QuotaNo, a.LoanAccount, a.LabelCode from archivetransferdetails t left join archiveinfo a on t.archiveid = a.id where t.PId =:PId  and  t.ArchiveId = :ArchiveId ";

        //    return DBCache.DataBase.ExecuteEntity<ArchiveTransferDetails>(
        //        sql,
        //        DBCache.DataBase.CreatDbParameter("PId", pId),
        //        DBCache.DataBase.CreatDbParameter("ArchiveId", archiveId));
        //}


        //public static PagingResult<SelectArchiveModel> GetArchiveList(int page, int limit, string transferId)
        //{
        //    var sql = @"select a.*, b.IsChecked, o.name as OrgName, o.code as OrgCode from ArchiveInfo a 
        //                left join (select archiveid, 1 as IsChecked from ArchiveTransferDetails where pid = :transferId) b on a.id = b.archiveid
        //                left join orginfo o on a.orgid = o.id
        //                where a.id not in(
        //                        select atd.archiveid from ArchiveTransferDetails atd left join ArchiveTransferInfo at on atd.pid = at.id
        //                        where at.status <> 0 )
        //                    and (a.status = 0 or a.status is null)
        //                order by  b.IsChecked, a.orgid, a.quotano, a.loanaccount";

        //    var rCount = DBCache.DataBase.GetRecordCount(sql, DBCache.DataBase.CreatDbParameter("transferId", transferId));
        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<SelectArchiveModel>(page, limit, sql, DBCache.DataBase.CreatDbParameter("transferId", transferId));

        //    return new PagingResult<SelectArchiveModel>() { Count = rCount, Result = data };
        //}


        //public static PagingResult<SelectArchiveModel> GetArchiveList(int page, int limit, string orgId, string transferId)
        //{
        //    var sql = @"select a.*, b.IsChecked, o.name as OrgName, o.code as OrgCode from ArchiveInfo a 
        //                left join (select archiveid, 1 as IsChecked from ArchiveTransferDetails where pid = :transferId) b on a.id = b.archiveid
        //                left join orginfo o on a.orgid = o.id
        //                where a.id not in(
        //                        select atd.archiveid from ArchiveTransferDetails atd left join ArchiveTransferInfo at on atd.pid = at.id
        //                        where at.status <> 0 )
        //                    and (a.status = 0 or a.status is null)
        //                    and a.OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) 
        //                order by  b.IsChecked, a.orgid, a.quotano, a.loanaccount";


        //    var rCount = DBCache.DataBase.GetRecordCount(sql, 
        //        DBCache.DataBase.CreatDbParameter("transferId", transferId),
        //        DBCache.DataBase.CreatDbParameter("OrgId", orgId));

        //    var data = DBCache.DataBase.ExecuteEntityListByPageing<SelectArchiveModel>(page, limit, sql,
        //        DBCache.DataBase.CreatDbParameter("transferId", transferId),
        //        DBCache.DataBase.CreatDbParameter("OrgId", orgId));

        //    return new PagingResult<SelectArchiveModel>() { Count = rCount, Result = data };
        //}


        #region 【Add】
        //public static int Add(ArchiveTransferInfo info, List<ArchiveTransferDetails> details)
        //{
        //    return DBCache.DataBase.ExecuteNonQuery((cmd) =>
        //    {
        //        AddInfo(cmd, info);
        //        AddDetails(cmd, details);
        //        return 1;
        //    });
        //}


        //private static int AddInfo(DbCommand cmd, ArchiveTransferInfo info)
        //{
        //    if (info == null) return 1;
        //    var sql = "insert into ArchiveTransferInfo (Id, OrgId, Handover, Receiver, Status, TransferDate) values(:Id, :OrgId, :Handover, :Receiver, :Status, :TransferDate)";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", info.Id));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("OrgId", info.OrgId));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Handover", info.Handover));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Receiver", info.Receiver));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", 0));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("TransferDate", info.TransferDate));

        //    return cmd.ExecuteNonQuery();
        //}


        //private static int AddDetails(DbCommand cmd, List<ArchiveTransferDetails> detailsList)
        //{
        //    if (detailsList == null || detailsList.Count == 0) return 1;
        //    var sql = "insert into ArchiveTransferDetails (Id, PId, ArchiveId, STATUS) values(:Id, :PId, :ArchiveId, 0)";
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

        #endregion


        #region 【Update】
        //public static int Update(ArchiveTransferInfo info, List<ArchiveTransferDetails> details)
        //{
        //    return DBCache.DataBase.ExecuteNonQuery((cmd) =>
        //    {
        //        UpdateInfo(cmd, info);

        //        UpdateDetails(cmd, details);

        //        return 1;
        //    });
        //}


        //private static int UpdateInfo(DbCommand cmd, ArchiveTransferInfo info)
        //{
        //    var sql = @"update ArchiveTransferInfo set OrgId = :OrgId, Handover = :Handover, Receiver = :Receiver, 
        //                    TransferDate = :TransferDate, Status = :Status where Id = :Id ";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("OrgId", info.OrgId));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Handover", info.Handover));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Receiver", info.Receiver));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("TransferDate", info.TransferDate));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", info.Status));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", info.Id));

        //    return cmd.ExecuteNonQuery();
        //}


        //private static int UpdateDetails(DbCommand cmd, List<ArchiveTransferDetails> detailsList)
        //{
        //    return 1;
        //}


        #endregion


        //public static int EditDetails(ArchiveTransferDetails data)
        //{
        //    return DBCache.DataBase.ExecuteNonQuery((cmd) =>
        //    {
        //        var ret = 0;

        //        ret = UpdateDetails(cmd, new List<ArchiveTransferDetails>() { data });
        //        if (ret > 0) return ret;

        //        return AddDetails(cmd, new List<ArchiveTransferDetails>() { data });
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
        //    var sql = "delete from ArchiveTransferInfo where Id = :Id and Status = 0 ";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", id));
        //    return cmd.ExecuteNonQuery();
        //}


        //private static int DelDetail(DbCommand cmd, string transferId)
        //{
        //    var sql = "delete from ArchiveTransferDetails where PId = :PId ";
        //    cmd.CommandText = sql;

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("PId", transferId));
        //    return cmd.ExecuteNonQuery();
        //}


        //public static int DelDetail(string transferId, string archiveId)
        //{
        //    var sql = "delete from ArchiveTransferDetails where PId = :PId and ArchiveId = :ArchiveId ";
        //    return DBCache.DataBase.ExecuteNonQuery(
        //        sql,
        //        DBCache.DataBase.CreatDbParameter("PId", transferId),
        //        DBCache.DataBase.CreatDbParameter("ArchiveId", archiveId));
        //}




        //#region 审核通过，驳回





        //public static int RollBack(string id)
        //{
        //    var sql = @"update ArchiveTransferInfo set Status = 0 where Id = :Id ";
        //    return DBCache.DataBase.ExecuteNonQuery(
        //        sql,
        //        DBCache.DataBase.CreatDbParameter("Id", id));
        //}



        //#endregion

        //public static List<InventoryDetail> GetTransferInventoryDetails(string pId)
        //{
        //    var sql = @"select t.Id, a.LabelCode from archivetransferdetails t
        //                left join ArchiveInfo a on t.archiveid = a.id where t.PID = :PID";
        //    return DBCache.DataBase.ExecuteEntityList<InventoryDetail>(sql, DBCache.DataBase.CreatDbParameter("PID", pId));
        //}


        //public static int VerifyData(List<ArchiveTransferDetails> details)
        //{
        //    if (details == null || details.Count == 0) return 0;
        //    return DBCache.DataBase.ExecuteNonQuery((cmd) =>
        //    {
        //        var ret = 0;
        //        foreach(var item in details)
        //        {
        //            ret += VerifyData(cmd, item);
        //        }

        //        return ret;
        //    });
        //}

        //private static int VerifyData(DbCommand cmd, ArchiveTransferDetails data)
        //{
        //    cmd.CommandText = "update archivetransferdetails set Status = :Status where Id = :Id";

        //    cmd.Parameters.Clear();
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Status", data.Status.GetHashCode()));
        //    cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", data.Id));

        //    return cmd.ExecuteNonQuery();
        //}

        #endregion
    }
}
