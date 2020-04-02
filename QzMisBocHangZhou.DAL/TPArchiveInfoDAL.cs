using QzMisBocHangZhou.Model;
using System.Collections.Generic;
using System.Data.Common;

namespace QzMisBocHangZhou.DAL
{
    public class TPArchiveInfoDAL
    {
        public static List<TPArchiveInfo> Get()
        {
            var sql = "select * from TPArchiveInfo t order by t.EditDate, t.ArchiveNo";
            return DBCache.DataBase.ExecuteEntityList<TPArchiveInfo>(sql);
        }


        public static TPArchiveInfo Get(string id)
        {
            var sql = "select * from TPArchiveInfo t where Id = :Id";
            return DBCache.DataBase.ExecuteEntity<TPArchiveInfo>(sql,
                DBCache.DataBase.CreatDbParameter("Id", id));

        }


        public static PagingResult<TPArchiveInfo> PagingQuery(int page, int limit, string keywords, string orgId)
        {
            var sql = @"select tpa.* from tparchiveinfo tpa 
                        left join orginfo org on tpa.orgcode = org.code  where 1=1 ";

            var pars = new List<DbParameter>();
            if (!string.IsNullOrWhiteSpace(orgId))
            {
                sql += @" and org.Id in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId) ";
                pars.Add(DBCache.DataBase.CreatDbParameter("OrgId", orgId));
            }

            if (!string.IsNullOrWhiteSpace(keywords))
            {
                sql += @" and (LoanAccount like :KeyWords
                        or QuotaNo like :KeyWords 
                        or ArchiveNo like :KeyWords 
                        or GuaranteeNo like :KeyWords
                        or CustomerNo like :KeyWords)";

                pars.Add(DBCache.DataBase.CreatDbParameter("KeyWords", $"%{keywords.Trim()}%"));
            }

            sql += " order by tpa.EditDate, tpa.ArchiveNo ";
            var rCount = DBCache.DataBase.GetRecordCount(sql, pars.ToArray());
            var data = DBCache.DataBase.ExecuteEntityListByPageing<TPArchiveInfo>(page, limit, sql, pars.ToArray());

            return new PagingResult<TPArchiveInfo>() { Count = rCount, Result = data };
        }


        public static List<TPArchiveInfo> GetByFuzzyMatching(TPArchiveInfo data)
        {
            var sql = @"select * from TPArchiveInfo where QuotaNo = :QuotaNo or LoanAccount = :LoanAccount or ArchiveNo = :ArchiveNo";

            return DBCache.DataBase.ExecuteEntityList<TPArchiveInfo>(sql,
                    DBCache.DataBase.CreatDbParameter("QuotaNo", data.QuotaNo),
                    DBCache.DataBase.CreatDbParameter("LoanAccount", data.LoanAccount),
                    DBCache.DataBase.CreatDbParameter("ArchiveNo", data.ArchiveNo)
                );
        }


        public static int Add(TPArchiveInfo data)
        {
            var sql = @"insert into TPArchiveInfo
                        ( 
                            Id, OfficialArchiveId, GuaranteeNo, LoanAccount, QuotaNo, ArchiveNo, StorageLocation, OrgCode, OrgName, 
                            ProductCode, Borrower, LoanAmount, GuaranteeType, ArchiveStatus, GuaranteeCredentialNo, Transactor, 
                            LoanReleaseDate, MortgageValue, ContractNo, HouseLocation, AccountManager, PreReturnDate, ReturnDate, EditDate, CustomerNo
                        ) 
                        values 
                        (
                            :Id, :OfficialArchiveId, :GuaranteeNo, :LoanAccount, :QuotaNo, :ArchiveNo, :StorageLocation, :OrgCode, :OrgName, 
                            :ProductCode, :Borrower, :LoanAmount, :GuaranteeType, :ArchiveStatus, :GuaranteeCredentialNo, :Transactor, 
                            :LoanReleaseDate, :MortgageValue, :ContractNo, :HouseLocation, :AccountManager, :PreReturnDate, :ReturnDate, :EditDate, :CustomerNo
                        )";


            return DBCache.DataBase.ExecuteNonQuery(sql,
                    DBCache.DataBase.CreatDbParameter("Id", data.Id),
                    DBCache.DataBase.CreatDbParameter("OfficialArchiveId", data.OfficialArchiveId),
                    DBCache.DataBase.CreatDbParameter("GuaranteeNo", data.GuaranteeNo),
                    DBCache.DataBase.CreatDbParameter("LoanAccount", data.LoanAccount),
                    DBCache.DataBase.CreatDbParameter("QuotaNo", data.QuotaNo),
                    DBCache.DataBase.CreatDbParameter("ArchiveNo", data.ArchiveNo),
                    DBCache.DataBase.CreatDbParameter("StorageLocation", data.StorageLocation),
                    DBCache.DataBase.CreatDbParameter("OrgCode", data.OrgCode),
                    DBCache.DataBase.CreatDbParameter("OrgName", data.OrgName),
                    DBCache.DataBase.CreatDbParameter("ProductCode", data.ProductCode),
                    DBCache.DataBase.CreatDbParameter("Borrower", data.Borrower),
                    DBCache.DataBase.CreatDbParameter("LoanAmount", data.LoanAmount),
                    DBCache.DataBase.CreatDbParameter("GuaranteeType", data.GuaranteeType),
                    DBCache.DataBase.CreatDbParameter("ArchiveStatus", data.ArchiveStatus),
                    DBCache.DataBase.CreatDbParameter("GuaranteeCredentialNo", data.GuaranteeCredentialNo),
                    DBCache.DataBase.CreatDbParameter("Transactor", data.Transactor),
                    DBCache.DataBase.CreatDbParameter("LoanReleaseDate", data.LoanReleaseDate),
                    DBCache.DataBase.CreatDbParameter("MortgageValue", data.MortgageValue),
                    DBCache.DataBase.CreatDbParameter("ContractNo", data.ContractNo),
                    DBCache.DataBase.CreatDbParameter("HouseLocation", data.HouseLocation),
                    DBCache.DataBase.CreatDbParameter("AccountManager", data.AccountManager),
                    DBCache.DataBase.CreatDbParameter("PreReturnDate", data.PreReturnDate),
                    DBCache.DataBase.CreatDbParameter("ReturnDate", data.ReturnDate),
                    DBCache.DataBase.CreatDbParameter("EditDate", data.EditDate),
                    DBCache.DataBase.CreatDbParameter("CustomerNo", data.CustomerNo)
                );
        }


        public static int Update(TPArchiveInfo data)
        {
            var sql = @"update TPArchiveInfo set 
                            GuaranteeNo = :GuaranteeNo, LoanAccount = :LoanAccount, QuotaNo = :QuotaNo, ArchiveNo = :ArchiveNo, 
                            StorageLocation = :StorageLocation, OrgCode = :OrgCode, OrgName = :OrgName, ProductCode = :ProductCode, 
                            Borrower = :Borrower, LoanAmount = :LoanAmount, GuaranteeType = :GuaranteeType, ArchiveStatus = :ArchiveStatus, 
                            GuaranteeCredentialNo = :GuaranteeCredentialNo, Transactor = :Transactor, LoanReleaseDate = :LoanReleaseDate, 
                            MortgageValue = :MortgageValue, ContractNo = :ContractNo, HouseLocation = :HouseLocation, AccountManager = :AccountManager, 
                            PreReturnDate = :PreReturnDate, ReturnDate = :ReturnDate, EditDate = :EditDate, CustomerNo = :CustomerNo 
                        where Id = :Id";

            return DBCache.DataBase.ExecuteNonQuery(sql,
                    DBCache.DataBase.CreatDbParameter("GuaranteeNo", data.GuaranteeNo),
                    DBCache.DataBase.CreatDbParameter("LoanAccount", data.LoanAccount),
                    DBCache.DataBase.CreatDbParameter("QuotaNo", data.QuotaNo),
                    DBCache.DataBase.CreatDbParameter("ArchiveNo", data.ArchiveNo),
                    DBCache.DataBase.CreatDbParameter("StorageLocation", data.StorageLocation),
                    DBCache.DataBase.CreatDbParameter("OrgCode", data.OrgCode),
                    DBCache.DataBase.CreatDbParameter("OrgName", data.OrgName),
                    DBCache.DataBase.CreatDbParameter("ProductCode", data.ProductCode),
                    DBCache.DataBase.CreatDbParameter("Borrower", data.Borrower),
                    DBCache.DataBase.CreatDbParameter("LoanAmount", data.LoanAmount),
                    DBCache.DataBase.CreatDbParameter("GuaranteeType", data.GuaranteeType),
                    DBCache.DataBase.CreatDbParameter("ArchiveStatus", data.ArchiveStatus),
                    DBCache.DataBase.CreatDbParameter("GuaranteeCredentialNo", data.GuaranteeCredentialNo),
                    DBCache.DataBase.CreatDbParameter("Transactor", data.Transactor),
                    DBCache.DataBase.CreatDbParameter("LoanReleaseDate", data.LoanReleaseDate),
                    DBCache.DataBase.CreatDbParameter("MortgageValue", data.MortgageValue),
                    DBCache.DataBase.CreatDbParameter("ContractNo", data.ContractNo),
                    DBCache.DataBase.CreatDbParameter("HouseLocation", data.HouseLocation),
                    DBCache.DataBase.CreatDbParameter("AccountManager", data.AccountManager),
                    DBCache.DataBase.CreatDbParameter("PreReturnDate", data.PreReturnDate),
                    DBCache.DataBase.CreatDbParameter("ReturnDate", data.ReturnDate),
                    DBCache.DataBase.CreatDbParameter("EditDate", data.EditDate),
                    DBCache.DataBase.CreatDbParameter("CustomerNo", data.CustomerNo),
                    DBCache.DataBase.CreatDbParameter("Id", data.Id)
                );
        }


        public static int Bind(string id, string officialArchiveId)
        {
            var sql = @"update TPArchiveInfo set OfficialArchiveId = :OfficialArchiveId where Id = :Id ";
            return DBCache.DataBase.ExecuteNonQuery(sql,
                DBCache.DataBase.CreatDbParameter("OfficialArchiveId", officialArchiveId),
                DBCache.DataBase.CreatDbParameter("Id", id));
        }

    }
}
