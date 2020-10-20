using QzMisBocHangZhou.DAL;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QzMisBocHangZhou.Biz
{
    public class InventoryInfoBiz
    {
        public static PagingResult<ArchiveInfo> GetInventoryArchiveList(int page, int limit, string orgId)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return InventoryInfoDAL.GetInventoryArchiveList(page, limit, orgId);
        }

        public static InventoryInfo Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return new InventoryInfo();
            return InventoryInfoDAL.Get(id);
        }

        public static PagingResult<InventoryInfo> Get(int page, int limit)
        {
            return InventoryInfoDAL.Get(page, limit);
        }

        public static byte[] Export(string orgId, UserInfo user)
        {
            var info = CreatNewInfo(user);
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;
            var details = InventoryInfoDAL.GetInventoryArchiveList(orgId);

            if (details == null || details.Count == 0) return new byte[1];

            StringBuilder sb = new StringBuilder();
            foreach (var item in details)
            {
                item.Id = Guid.NewGuid().ToString();
                item.InventoryId = info.Id;
                item.Status = VerifyType.未核对;
                
                sb.Append("|".PadRight(12, '|')).Append($"{item.LabelCode}".PadRight(18, '0')).Append("|").Append("1".PadLeft(18, '0')).Append("|".PadRight(5, '|')).AppendLine();
            }
            InventoryInfoDAL.Add(info, details);

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        private static InventoryInfo CreatNewInfo(UserInfo user)
        {
            var info = new InventoryInfo()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                OrgId = user.OrgId,
                InventoryName = DateTime.Now.ToString("yyyyMMddHHmmss"),
                IsLocked = 0
            };

            return info;
        }

        public static bool Import(string id, string fileName)
        {
            var data = File.ReadAllLines(fileName);

            var info = InventoryInfoDAL.Get(id);
            var fullDetails = InventoryInfoDAL.GetInventoryDetails(id);

            foreach (var item in data)
            {
                if (string.IsNullOrWhiteSpace(item) || item.Length < 18) continue;
                var detailInfo = fullDetails.Find(p => p.LabelCode.PadRight(18, '0') == item.Substring(0, 18));
                if (detailInfo == null) continue;
                Enum.TryParse(item.Substring(41, 1), out VerifyType type);
                detailInfo.Status = type;
                detailInfo.InventoryTime = DateTime.Now;
            }

            if (fullDetails.Count == fullDetails.Count(p => p.Status == VerifyType.成功))
            {
                info.IsLocked = 1;
                info.EndTime = DateTime.Now;
            }

            var details = fullDetails.FindAll(p => p.Status == VerifyType.成功);

            return InventoryInfoDAL.ImportInventoryData(info, details) > 0;
        }

        public static PagingResult<InventoryDetail> GetDetails(int page, int limit, string tId)
        {
            if (string.IsNullOrWhiteSpace(tId)) return new PagingResult<InventoryDetail>();

            return InventoryInfoDAL.GetDetails(page, limit, tId);
        }
    }
}
