using QzMisBocHangZhou.DAL;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Biz
{
    public class ArchiveSettleInfoBiz
    {
        public static PagingResult<ArchiveSettleInfo> GetPreSettle(int page, int limit, string orgId, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return ArchiveSettleInfoDAL.GetPreSettle(page, limit, orgId, keyWords);
        }

        public static PagingResult<ArchiveSettleInfo> GetPreReview(int page, int limit, string orgId, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return ArchiveSettleInfoDAL.GetPreReview(page, limit, orgId, keyWords);
        }

        public static PagingResult<ArchiveSettleInfo> GetPreOut(int page, int limit, string orgId, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return ArchiveSettleInfoDAL.GetPreOut(page, limit, orgId, keyWords);
        }

        public static List<ArchiveSettleInfo> GetPreOut()
        {
            return ArchiveSettleInfoDAL.GetPreOut();
        }

        public static bool SubmitReview(string tId, string usedBy, DateTime? settleDate, UserInfo user)
        {
            if (string.IsNullOrWhiteSpace(tId)) return false;
            if (user == null) return false;
            if (string.IsNullOrWhiteSpace(user.OrgId)) return false;

            if (!settleDate.HasValue) settleDate = DateTime.Now;
            
            var data = new ArchiveSettleInfo()
            {
                Id = Guid.NewGuid().ToString(),
                ArchiveId = tId,
                SettleDate = settleDate,
                UsedBy = usedBy,
                OrgId = user.OrgId,
                SettlePeople = user.RealName,
                Status = 0,
            };

            return ArchiveSettleInfoDAL.SubmitReview(data) > 0;
        }

        public static bool RollBack(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            var data = ArchiveSettleInfoDAL.Get(id);
            if (data == null || string.IsNullOrWhiteSpace(data.Id)) return false;
            if (string.IsNullOrWhiteSpace(data.ArchiveId)) return false;

            return ArchiveSettleInfoDAL.RollBack(data) > 0;
        }

        public static bool PassReview(string id, UserInfo user)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            var data = ArchiveSettleInfoDAL.Get(id);
            if (data == null || string.IsNullOrWhiteSpace(data.Id)) return false;
            if (string.IsNullOrWhiteSpace(data.ArchiveId)) return false;
            return ArchiveSettleInfoDAL.PassReview(data) > 0;
        }

        public static bool SettleOut(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            return ArchiveSettleInfoDAL.SettleOut(id) > 0;
        }
    }
}
