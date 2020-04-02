using QzMisBocHangZhou.DAL;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QzMisBocHangZhou.Biz
{
    public class TPArchiveInfoBiz
    {
        public static List<TPArchiveInfo> Get()
        {
            return TPArchiveInfoDAL.Get();
        }


        public static TPArchiveInfo Get(string id)
        {
            return TPArchiveInfoDAL.Get(id);
        }


        public static PagingResult<TPArchiveInfo> PagingQuery(int page, int limit, string keywords, string orgId)
        {
            if (string.IsNullOrWhiteSpace(orgId) || orgId.Equals(OrgInfo.RootId, StringComparison.OrdinalIgnoreCase)) orgId = string.Empty;
            keywords = keywords?.Trim();

            return TPArchiveInfoDAL.PagingQuery(page, limit, keywords, orgId);
        }


        //TODO: FTP导入时判断新增或更新数据(存在问题，数据有概率匹配错误，或无法匹配)
        public static bool Edit(TPArchiveInfo data)
        {
            if (Update(data)) return true;

            return Add(data);
        }

        private static bool Add(TPArchiveInfo data)
        {
            data.Id = Guid.NewGuid().ToString();
            if (!RequiredData(data)) return false;

            return TPArchiveInfoDAL.Add(data) > 0;
        }


        private static bool Update(TPArchiveInfo data)
        {
            var id = GetExactMatchId(data);
            if (string.IsNullOrWhiteSpace(id)) return false;

            data.Id = id;
            if (!RequiredData(data)) return false;

            return TPArchiveInfoDAL.Update(data) > 0;

        }


        public static bool Bind(string id, string officialArchiveId)
        {
            return TPArchiveInfoDAL.Bind(id, officialArchiveId) > 0;
        }


        private static bool RequiredData(TPArchiveInfo data)
        {
            if (string.IsNullOrWhiteSpace(data.Id)) return false;
            if (string.IsNullOrWhiteSpace(data.LoanAccount) && string.IsNullOrWhiteSpace(data.QuotaNo)) return false;

            return true;
        }


        /// <summary>
        /// 精确匹配
        /// </summary>
        /// <param name="conditionData"></param>
        /// <returns></returns>
        private static string GetExactMatchId(TPArchiveInfo conditionData)
        {
            var fuzzyData = TPArchiveInfoDAL.GetByFuzzyMatching(conditionData);
            if (fuzzyData == null || fuzzyData.Count == 0) return string.Empty;
            if (fuzzyData.Count == 1) return fuzzyData[0].Id;

            var sameItemCount = new List<int>();
            foreach (var item in fuzzyData)
            {
                var ret = GetSimilarItemsNum(item, conditionData);
                sameItemCount.Add(ret);
            }

            var dataIdx = sameItemCount.IndexOf(sameItemCount.Max());
            return dataIdx == -1 ? string.Empty : fuzzyData[dataIdx].Id;
        }


        /// <summary>
        /// 相似项权重计算
        /// </summary>
        /// <param name="funzzyData"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private static int GetSimilarItemsNum(TPArchiveInfo funzzyData, TPArchiveInfo condition)
        {
            var ret = 0;

            //贷款账号权重:40
            if (funzzyData.LoanAccount.Trim().Equals(condition.LoanAccount, StringComparison.OrdinalIgnoreCase)) ret += 40;

            //额度号权重:20
            if (funzzyData.QuotaNo.Trim().Equals(condition.QuotaNo, StringComparison.OrdinalIgnoreCase)) ret += 20;

            //第三方系统档案号权重:15
            if (funzzyData.ArchiveNo.Trim().Equals(condition.ArchiveNo, StringComparison.OrdinalIgnoreCase)) ret += 15;

            //第三方系统担保品编号权重:10
            if (funzzyData.GuaranteeNo.Trim().Equals(condition.GuaranteeNo, StringComparison.OrdinalIgnoreCase)) ret += 10;

            //机构号权重:3
            if (funzzyData.OrgCode.Trim().Equals(condition.OrgCode, StringComparison.OrdinalIgnoreCase)) ret += 3;

            return ret;
        }
    }
}
