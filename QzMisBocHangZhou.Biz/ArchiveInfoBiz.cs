﻿using QzMisBocHangZhou.DAL;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Biz
{
    public class ArchiveInfoBiz
    {
        public static List<ArchiveInfo> Get()
        {
            return ArchiveInfoDAL.Get();
        }


        public static ArchiveInfo Get(string id)
        {
            return ArchiveInfoDAL.Get(id);
        }


        public static PagingResult<ArchiveInfo> PagingQuery(int page, int limit, string keywords, string orgId, int status)
        {
            if (string.IsNullOrWhiteSpace(orgId) || orgId.Equals(OrgInfo.RootId, StringComparison.OrdinalIgnoreCase)) orgId = string.Empty;
            keywords = keywords?.Trim();

            return ArchiveInfoDAL.PagingQuery(page, limit, keywords, orgId, status);
        }


        //public static ArchiveInfo GetFromTPData(string tpId)
        //{
        //    var tpData = TPArchiveInfoBiz.Get(tpId);
        //    if (string.IsNullOrWhiteSpace(tpData.Id)) return new ArchiveInfo();
        //    var orgInfo = OrgInfoBiz.GetByCode(tpData.OrgCode);
        //    var result = new ArchiveInfo
        //    {
        //        TPId = tpData.Id,
        //        OrgCode = tpData.OrgCode,
        //        OrgName = tpData.OrgName,
        //        OrgId = orgInfo.Id,
        //        AccountManager = tpData.AccountManager,
        //        Borrower = tpData.Borrower,
        //        GuaranteeCrdNo = tpData.GuaranteeCredentialNo,
        //        GuaranteeType = tpData.GuaranteeType,
        //        LoanAccount = tpData.LoanAccount,
        //        LoanAmount = tpData.LoanAmount,
        //        LoanReleaseDate = tpData.LoanReleaseDate,
        //        MortgageDetailsInfo = tpData.HouseLocation,
        //        MortgageValue = tpData.MortgageValue,
        //        ProductCode = tpData.ProductCode,
        //        QuotaNo = tpData.QuotaNo,
        //        StorageLocation = tpData.StorageLocation,
        //        TPArchiveNo = tpData.ArchiveNo,
        //        TPContractNo = tpData.ContractNo,
        //        TPGuaranteeNo = tpData.GuaranteeNo,
        //        Transactor = tpData.Transactor
        //    };

        //    return result;
        //}


        public static bool Add(ArchiveInfo data)
        {
            data.Id = Guid.NewGuid().ToString();
            data.CreateDate = DateTime.Now;
            data.LastEditDate = data.CreateDate;

            if (!RequiredData(data)) return false;

            var ret = ArchiveInfoDAL.Add(data) > 0;
            //if(ret && !string.IsNullOrWhiteSpace(data.TPId))
            //{
            //    TPArchiveInfoBiz.Bind(data.TPId, data.Id);
            //}
            return ret;
        }


        public static bool Update(ArchiveInfo data)
        {
            data.LastEditDate = DateTime.Now;

            if (!RequiredData(data)) return false;

            return ArchiveInfoDAL.Update(data) > 0;

        }


        private static bool RequiredData(ArchiveInfo data)
        {
            if (string.IsNullOrWhiteSpace(data.Id)) return false;
            if (string.IsNullOrWhiteSpace(data.OrgId) || data.OrgId.Equals("00000", StringComparison.OrdinalIgnoreCase)) return false;
            if (string.IsNullOrWhiteSpace(data.LoanAccount) && string.IsNullOrWhiteSpace(data.QuotaNo)) return false;
            
            return true;
        }
    }
}