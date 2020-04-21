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
        public static List<ArchiveBorrowInfo> GetBorrowTimeOut(string orgId, string guaranteeType, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;
            return ReportDAL.GetBorrowTimeOut(orgId, guaranteeType, keyWords);
        }


        public static List<ArchiveTransferInfo> GetTransferTimeOut(string orgId, string guaranteeType, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            var day = BaseDataDAL.GetDay("移交");

            return ReportDAL.GetTransferTimeOut(orgId, day, guaranteeType, keyWords);
        }

        public static List<ArchiveSettleInfo> GetSettleTimeOut(string orgId, string guaranteeType, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            var day = BaseDataDAL.GetDay("结清未领取");
            return ReportDAL.GetSettleTimeOut(orgId, day, guaranteeType, keyWords);
        }        
    }
}
