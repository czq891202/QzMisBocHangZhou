using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.DAL
{
    public class ArchiveInfoDAL
    {
        #region 【查询】
        public static List<ArchiveInfo> Get()
        {
            var sql = "select * from ArchiveInfo t order by t.CREATEDATE, t.ORGID desc";
            return DBCache.DataBase.ExecuteEntityList<ArchiveInfo>(sql);
        }


        public static ArchiveInfo Get(string id)
        {
            var sql = "select * from ArchiveInfo t where Id = :Id";
            return DBCache.DataBase.ExecuteEntity<ArchiveInfo>(sql,
                DBCache.DataBase.CreatDbParameter("Id", id));

        }


        public static PagingResult<ArchiveInfo> PagingQuery(int page, int limit, string keywords, string orgId, int status)
        {
            var sql = @"select tpa.*, org.Code as OrgCode, org.Name as OrgName from Archiveinfo tpa 
                        left join orginfo org on tpa.OrgId = org.Id  where 1=1 ";

            var pars = new List<DbParameter>();
            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and org.Id in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            if (!string.IsNullOrWhiteSpace(keywords))
            {
                sql += @" and (LoanAccount like :KeyWords
                        or QuotaNo like :KeyWords 
                        or Borrower like :KeyWords 
                        or SeqNo like :KeyWords 
                        or CustomerNo like :keywords)";

                pars.Add(DBCache.DataBase.CreatDbParameter("KeyWords", $"%{keywords.Trim()}%"));
            }


            if (status == 0)
            {
                //未移交
                sql += $" and (STATUS = {ArchiveStatusType.草稿.GetHashCode()}) ";
            }
            else if(status == 1)
            {
                //在库
                sql += $" and (STATUS = {ArchiveStatusType.已入库.GetHashCode()}) ";
            }
            else if (status == 2)
            {
                //借阅
                sql += $" and (STATUS = {ArchiveStatusType.借阅出库.GetHashCode()}) ";
            }
            else if (status == 3)
            {
                //结清
                sql += $" and (STATUS = {ArchiveStatusType.已结清.GetHashCode()} or STATUS = {ArchiveStatusType.变更结清出库.GetHashCode()} or STATUS = {ArchiveStatusType.处置出库.GetHashCode()}) ";
            }
                       

            sql += " order by tpa.CREATEDATE, tpa.ORGID desc ";
            var rCount = DBCache.DataBase.GetRecordCount(sql, pars.ToArray());
            var data = DBCache.DataBase.ExecuteEntityListByPageing<ArchiveInfo>(page, limit, sql, pars.ToArray());

            return new PagingResult<ArchiveInfo>() { Count = rCount, Result = data };
        }


        #endregion


        #region 【新增】
        public static int Add(ArchiveInfo data)
        {
            var sql = @"insert into ArchiveInfo
                        ( 
                            Id, OrgId, QuotaNo, LoanAccount, ProductCode, LabelCode, Borrower, LoanAmount, 
                            GuaranteeType, GuaranteeCrdNo, Transactor, LoanReleaseDate, MortgageValue, TPContractNo, MortgageDetailsInfo, 
                            AccountManager, TPGuaranteeNo, TPArchiveNo, StorageLocation, Status, LastEditDate, CreateDate, CustomerNo, 
                            SeqNo, ProjectNo, CreditsNo, CoreProductNo, LvlFiveAuto, LvlFiveManual, LoanBalance, LoanTerm, LoanMaturityDate,
                            LoanStatus, LoanStatusEditDate, CreditStatus, CreditStatusEditDate, WriteOffStatus, SecuritizationStatus, LinkedAccount
                        ) 
                        values 
                        (
                            :Id, :OrgId, :QuotaNo, :LoanAccount, :ProductCode, :LabelCode, :Borrower, :LoanAmount, 
                            :GuaranteeType, :GuaranteeCrdNo, :Transactor, :LoanReleaseDate, :MortgageValue, :TPContractNo, :MortgageDetailsInfo, 
                            :AccountManager, :TPGuaranteeNo, :TPArchiveNo, :StorageLocation, :Status, :LastEditDate, :CreateDate, :CustomerNo, 
                            :SeqNo, :ProjectNo, :CreditsNo, :CoreProductNo, :LvlFiveAuto, :LvlFiveManual, :LoanBalance, :LoanTerm, :LoanMaturityDate,
                            :LoanStatus, :LoanStatusEditDate, :CreditStatus, :CreditStatusEditDate, :WriteOffStatus, :SecuritizationStatus, :LinkedAccount
                        )";


            return DBCache.DataBase.ExecuteNonQuery(sql,
                    DBCache.DataBase.CreatDbParameter("Id", data.Id),
                    DBCache.DataBase.CreatDbParameter("OrgId", data.OrgId),
                    DBCache.DataBase.CreatDbParameter("QuotaNo", data.QuotaNo),
                    DBCache.DataBase.CreatDbParameter("LoanAccount", data.LoanAccount),
                    DBCache.DataBase.CreatDbParameter("ProductCode", data.ProductCode),
                    DBCache.DataBase.CreatDbParameter("LabelCode", data.LabelCode),
                    DBCache.DataBase.CreatDbParameter("Borrower", data.Borrower),
                    DBCache.DataBase.CreatDbParameter("LoanAmount", data.LoanAmount),

                    DBCache.DataBase.CreatDbParameter("GuaranteeType", data.GuaranteeType),
                    DBCache.DataBase.CreatDbParameter("GuaranteeCrdNo", data.GuaranteeCrdNo),
                    DBCache.DataBase.CreatDbParameter("Transactor", data.Transactor),
                    DBCache.DataBase.CreatDbParameter("LoanReleaseDate", data.LoanReleaseDate),
                    DBCache.DataBase.CreatDbParameter("MortgageValue", data.MortgageValue),
                    DBCache.DataBase.CreatDbParameter("TPContractNo", data.TPContractNo),
                    DBCache.DataBase.CreatDbParameter("MortgageDetailsInfo", data.MortgageDetailsInfo),

                    DBCache.DataBase.CreatDbParameter("AccountManager", data.AccountManager),
                    DBCache.DataBase.CreatDbParameter("TPGuaranteeNo", data.TPGuaranteeNo),
                    DBCache.DataBase.CreatDbParameter("TPArchiveNo", data.TPArchiveNo),
                    DBCache.DataBase.CreatDbParameter("StorageLocation", data.StorageLocation),
                    DBCache.DataBase.CreatDbParameter("Status", 0),
                    DBCache.DataBase.CreatDbParameter("LastEditDate", data.LastEditDate),
                    DBCache.DataBase.CreatDbParameter("CreateDate", data.CreateDate),
                    DBCache.DataBase.CreatDbParameter("CustomerNo", data.CustomerNo),

                    DBCache.DataBase.CreatDbParameter("SeqNo", data.SeqNo),
                    DBCache.DataBase.CreatDbParameter("ProjectNo", data.ProjectNo),
                    DBCache.DataBase.CreatDbParameter("CreditsNo", data.CreditsNo),
                    DBCache.DataBase.CreatDbParameter("CoreProductNo", data.CoreProductNo),
                    DBCache.DataBase.CreatDbParameter("LvlFiveAuto", data.LvlFiveAuto),
                    DBCache.DataBase.CreatDbParameter("LvlFiveManual", data.LvlFiveManual),
                    DBCache.DataBase.CreatDbParameter("LoanBalance", data.LoanBalance),
                    DBCache.DataBase.CreatDbParameter("LoanTerm", data.LoanTerm),
                    DBCache.DataBase.CreatDbParameter("LoanMaturityDate", data.LoanMaturityDate),

                    DBCache.DataBase.CreatDbParameter("LoanStatus", data.LoanStatus),
                    DBCache.DataBase.CreatDbParameter("LoanStatusEditDate", data.LoanStatusEditDate),
                    DBCache.DataBase.CreatDbParameter("CreditStatus", data.CreditStatus),
                    DBCache.DataBase.CreatDbParameter("CreditStatusEditDate", data.CreditStatusEditDate),
                    DBCache.DataBase.CreatDbParameter("WriteOffStatus", data.WriteOffStatus),
                    DBCache.DataBase.CreatDbParameter("SecuritizationStatus", data.SecuritizationStatus),
                    DBCache.DataBase.CreatDbParameter("LinkedAccount", data.LinkedAccount)
                );
        }


        #endregion


        #region 【编辑】
        public static int Update(ArchiveInfo data)
        {
            var sql = GetUpdateSql();
            var pars = GetUpdateParas(data);
            return DBCache.DataBase.ExecuteNonQuery(sql, pars);
        }


        public static int Update(DbCommand cmd, ArchiveInfo data)
        {
            var sql = GetUpdateSql();
            var pars = GetUpdateParas(data);

            cmd.CommandText = sql;
            cmd.Parameters.Clear();
            cmd.Parameters.AddRange(pars);
            return cmd.ExecuteNonQuery();
        }

        private static string GetUpdateSql()
        {
            var sql = @"update ArchiveInfo set 
                            OrgId = :OrgId, QuotaNo = :QuotaNo, LoanAccount = :LoanAccount, ProductCode = :ProductCode, 
                            LabelCode = :LabelCode, Borrower = :Borrower, LoanAmount = :LoanAmount, 
                            GuaranteeType = :GuaranteeType, GuaranteeCrdNo = :GuaranteeCrdNo, Transactor = :Transactor, 
                            LoanReleaseDate = :LoanReleaseDate, MortgageValue = :MortgageValue, TPContractNo = :TPContractNo, 
                            MortgageDetailsInfo = :MortgageDetailsInfo, AccountManager = :AccountManager, TPGuaranteeNo = :TPGuaranteeNo, 
                            TPArchiveNo = :TPArchiveNo, LastEditDate = :LastEditDate, CustomerNo = :CustomerNo,
                            SeqNo = :SeqNo, ProjectNo = :ProjectNo, CreditsNo = :CreditsNo, CoreProductNo = :CoreProductNo, LvlFiveAuto = :LvlFiveAuto, 
                            LvlFiveManual = :LvlFiveManual, LoanBalance = :LoanBalance, LoanTerm = :LoanTerm, LoanMaturityDate = :LoanMaturityDate,
                            LoanStatus = :LoanStatus, LoanStatusEditDate = :LoanStatusEditDate, CreditStatus = :CreditStatus, CreditStatusEditDate = :CreditStatusEditDate, 
                            WriteOffStatus = :WriteOffStatus, SecuritizationStatus = :SecuritizationStatus, LinkedAccount = :LinkedAccount
                        where Id = :Id";

            return sql;
        }

        private static DbParameter[] GetUpdateParas(ArchiveInfo data)
        {
            var paras = new List<DbParameter>()
            {
                DBCache.DataBase.CreatDbParameter("OrgId", data.OrgId),
                DBCache.DataBase.CreatDbParameter("QuotaNo", data.QuotaNo),
                DBCache.DataBase.CreatDbParameter("LoanAccount", data.LoanAccount),
                DBCache.DataBase.CreatDbParameter("ProductCode", data.ProductCode),

                DBCache.DataBase.CreatDbParameter("LabelCode", data.LabelCode),
                DBCache.DataBase.CreatDbParameter("Borrower", data.Borrower),
                DBCache.DataBase.CreatDbParameter("LoanAmount", data.LoanAmount),

                DBCache.DataBase.CreatDbParameter("GuaranteeType", data.GuaranteeType),
                DBCache.DataBase.CreatDbParameter("GuaranteeCrdNo", data.GuaranteeCrdNo),
                DBCache.DataBase.CreatDbParameter("Transactor", data.Transactor),

                DBCache.DataBase.CreatDbParameter("LoanReleaseDate", data.LoanReleaseDate),
                DBCache.DataBase.CreatDbParameter("MortgageValue", data.MortgageValue),
                DBCache.DataBase.CreatDbParameter("TPContractNo", data.TPContractNo),

                DBCache.DataBase.CreatDbParameter("MortgageDetailsInfo", data.MortgageDetailsInfo),
                DBCache.DataBase.CreatDbParameter("AccountManager", data.AccountManager),
                DBCache.DataBase.CreatDbParameter("TPGuaranteeNo", data.TPGuaranteeNo),

                DBCache.DataBase.CreatDbParameter("TPArchiveNo", data.TPArchiveNo),
                DBCache.DataBase.CreatDbParameter("LastEditDate", data.LastEditDate),
                DBCache.DataBase.CreatDbParameter("CustomerNo", data.CustomerNo),

                DBCache.DataBase.CreatDbParameter("SeqNo", data.SeqNo),
                DBCache.DataBase.CreatDbParameter("ProjectNo", data.ProjectNo),
                DBCache.DataBase.CreatDbParameter("CreditsNo", data.CreditsNo),
                DBCache.DataBase.CreatDbParameter("CoreProductNo", data.CoreProductNo),
                DBCache.DataBase.CreatDbParameter("LvlFiveAuto", data.LvlFiveAuto),

                DBCache.DataBase.CreatDbParameter("LvlFiveManual", data.LvlFiveManual),
                DBCache.DataBase.CreatDbParameter("LoanBalance", data.LoanBalance),
                DBCache.DataBase.CreatDbParameter("LoanTerm", data.LoanTerm),
                DBCache.DataBase.CreatDbParameter("LoanMaturityDate", data.LoanMaturityDate),

                DBCache.DataBase.CreatDbParameter("LoanStatus", data.LoanStatus),
                DBCache.DataBase.CreatDbParameter("LoanStatusEditDate", data.LoanStatusEditDate),
                DBCache.DataBase.CreatDbParameter("CreditStatus", data.CreditStatus),
                DBCache.DataBase.CreatDbParameter("CreditStatusEditDate", data.CreditStatusEditDate),

                DBCache.DataBase.CreatDbParameter("WriteOffStatus", data.WriteOffStatus),
                DBCache.DataBase.CreatDbParameter("SecuritizationStatus", data.SecuritizationStatus),
                DBCache.DataBase.CreatDbParameter("LinkedAccount", data.LinkedAccount),
                DBCache.DataBase.CreatDbParameter("Id", data.Id)
            };

            return paras.ToArray();
        }


        #endregion

    }
}
