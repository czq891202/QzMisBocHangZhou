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
        public static List<ArchiveBorrowInfo> GetBorrowTimeOut(string orgId)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;
            return ReportDAL.GetBorrowTimeOut(orgId);
        }


        public static List<ArchiveTransferInfo> GetTransferTimeOut(string orgId)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            var day = BaseDataDAL.GetDay("移交");

            return ReportDAL.GetTransferTimeOut(orgId, day);
        }

        public static List<ArchiveSettleInfo> GetSettleTimeOut(string orgId)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            var day = BaseDataDAL.GetDay("结清未领取");
            return ReportDAL.GetSettleTimeOut(orgId, day);
        }


        //public static List<ReportBorrow> GetByYearMonth(DateTime date)
        //{
        //    return ReportDAL.GetByYearMonth(date);
        //}


        //public static List<ReportBorrow> GetByDay(DateTime date)
        //{
        //    return ReportDAL.GetByDay(date);
        //}


        //public static List<ReportArchive> GetArchiveTotal()
        //{
        //    return ReportDAL.GetArchiveTotal();
        //}
    }
}
