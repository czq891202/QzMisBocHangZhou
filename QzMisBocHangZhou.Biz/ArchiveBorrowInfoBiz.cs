using QzMisBocHangZhou.DAL;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Biz
{
    public class ArchiveBorrowInfoBiz
    {
        public static ArchiveBorrowInfo Get(string id)
        {
            return ArchiveBorrowInfoDAL.Get(id);
        }
        //可借阅清单
        public static PagingResult<ArchiveBorrowInfo> GetPreBorrow(int page, int limit, string orgId, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return ArchiveBorrowInfoDAL.GetPreBorrow(page, limit, orgId, keyWords);
        }
        //借阅待审核清单
        public static PagingResult<ArchiveBorrowInfo> GetPreReview(int page, int limit, string orgId, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return ArchiveBorrowInfoDAL.GetPreReview(page, limit, orgId, keyWords);
        }
        //借阅待出库清单
        public static PagingResult<ArchiveBorrowInfo> GetPreOut(int page, int limit, string orgId, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return ArchiveBorrowInfoDAL.GetPreOut(page, limit, orgId, keyWords);
        }

        public static List<ArchiveBorrowInfo> GetPreOut()
        {
            return ArchiveBorrowInfoDAL.GetPreOut();
        }
        //借阅待审核清单导出
        public static List<ArchiveBorrowInfo> GetExcelData(string orgId)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return ArchiveBorrowInfoDAL.GetExcelData(orgId);
        }
        //提交借阅
        public static bool SubmitReview(string tId, string usedBy, DateTime? borrowDate, UserInfo user)
        {
            if (string.IsNullOrWhiteSpace(tId)) return false;
            if (user == null) return false;
            if (string.IsNullOrWhiteSpace(user.OrgId)) return false;
            
            if (!borrowDate.HasValue) borrowDate = DateTime.Now;
            var day = BaseDataDAL.GetDay(usedBy);
            if (day == 0) day = 15;

            var data = new ArchiveBorrowInfo()
            {
                Id = Guid.NewGuid().ToString(),
                ArchiveId = tId,
                BorrowDate = borrowDate,
                UsedBy = usedBy,
                OrgId = user.OrgId,
                Borrower = user.RealName,
                PreReturnDate = borrowDate.Value.AddDays(day),
                Status = 0,
            };

            ArchiveBorrowInfoDAL.SubmitReview(data);

            return ArchiveInfoDAL.ChangeArchiveStatus(data.ArchiveId,ArchiveStatusType.借阅审核中) > 0;
        }
        //借阅撤回/驳回
        public static bool RollBack(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            var data = ArchiveBorrowInfoDAL.Get(id);
            if (data == null) return false;
            if (string.IsNullOrWhiteSpace(data.Id)) return false;
            if (string.IsNullOrWhiteSpace(data.ArchiveId)) return false;

            return ArchiveBorrowInfoDAL.RollBack(data) > 0;
        }
        //审核通过
        public static bool PassReview(string id, UserInfo user)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            var data = ArchiveBorrowInfoDAL.Get(id);
            if (data == null) return false;
            if (string.IsNullOrWhiteSpace(data.Id)) return false;
            if (string.IsNullOrWhiteSpace(data.ArchiveId)) return false;

            return ArchiveBorrowInfoDAL.PassReview(data) > 0;
        }
        //借阅出库
        public static bool BorrowOut(string id, UserInfo user)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            var data = ArchiveBorrowInfoDAL.Get(id);
            if (data == null) return false;
            if (string.IsNullOrWhiteSpace(data.Id)) return false;
            if (string.IsNullOrWhiteSpace(data.ArchiveId)) return false;

            return ArchiveBorrowInfoDAL.BorrowOut(data) > 0;
        }
        //变更入库
        public static bool ChangeIn(ArchiveInfo data, string borrowId)
        {
            if (string.IsNullOrWhiteSpace(data.Id)) return false;
            if (string.IsNullOrWhiteSpace(data.LabelCode)) return false;

            var borrowData = ArchiveBorrowInfoDAL.Get(borrowId);

            return ArchiveBorrowInfoDAL.ChangeIn(data, borrowData) > 0;
        }
    }
}
