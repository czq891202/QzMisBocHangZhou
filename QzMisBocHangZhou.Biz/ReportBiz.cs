using QzMisBocHangZhou.DAL;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Biz
{
    public class ReportBiz
    {
        //借阅超时
        public static List<ArchiveBorrowInfo> GetBorrowTimeOut(string orgId, string guaranteeType, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;
            return ReportDAL.GetBorrowTimeOut(orgId, guaranteeType, keyWords);
        }
        //移交超时
        public static List<ArchiveTransferInfo> GetTransferTimeOut(string orgId, string guaranteeType, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            var day = BaseDataDAL.GetDay("移交");

            return ReportDAL.GetTransferTimeOut(orgId, day, guaranteeType, keyWords);
        }
        //结清超时
        public static List<ArchiveSettleInfo> GetSettleTimeOut(string orgId, string guaranteeType, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            var day = BaseDataDAL.GetDay("结清未领取");
            return ReportDAL.GetSettleTimeOut(orgId, day, guaranteeType, keyWords);
        }
        //档案追溯
        public static PagingResult<ArchiveInfoReport> GetArchiveInfoTime(int page, int limit, string orgId, string guaranteeType, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;
            return ReportDAL.GetArchiveInfoTime(page, limit, orgId, guaranteeType, keyWords);
        }
    }
}
