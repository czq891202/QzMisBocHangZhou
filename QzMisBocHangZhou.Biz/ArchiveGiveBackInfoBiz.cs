using QzMisBocHangZhou.DAL;
using QzMisBocHangZhou.Model;
using System;

namespace QzMisBocHangZhou.Biz
{
    public class ArchiveGiveBackInfoBiz
    {
        public static ArchiveBorrowInfo Get(string id)
        {
            return ArchiveBorrowInfoDAL.Get(id);
        }

        public static PagingResult<ArchiveBorrowInfo> GetPreGiveBack(int page, int limit, string orgId, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return ArchiveBorrowInfoDAL.GetPreGiveBack(page, limit, orgId, keyWords);
        }

        public static bool SubmitGiveBack(string bId, DateTime? givebackDate, UserInfo user)
        {
            if (string.IsNullOrWhiteSpace(bId)) return false;

            if (!givebackDate.HasValue) givebackDate = DateTime.Now;

            var data = new ArchiveBorrowInfo()
            {
                Id = bId,
                PreReturnDate = givebackDate,
                Status = 3
            };

            return ArchiveBorrowInfoDAL.SubmitGiveBack(data) > 0;
        }

        public static PagingResult<ArchiveBorrowInfo> GetGiveBackReview(int page, int limit, string orgId, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return ArchiveBorrowInfoDAL.GetGiveBackReview(page, limit, orgId, keyWords);
        }

        public static bool GiveBackRollBack(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            return ArchiveBorrowInfoDAL.GiveBackRollBack(id) > 0;
        }

        public static bool Returned(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            var data = ArchiveBorrowInfoDAL.Get(id);
            if (data == null) return false;
            if (string.IsNullOrWhiteSpace(data.Id)) return false;
            if (string.IsNullOrWhiteSpace(data.ArchiveId)) return false;

            return ArchiveBorrowInfoDAL.Returned(data) > 0;
        }

    }
}
