using QzMisBocHangZhou.DAL;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Biz
{
    public class ArchiveTransferInfoBiz
    {
        public static PagingResult<ArchiveTransferInfo> GetPreTransfer(int page, int limit, string orgId, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return ArchiveTransferInfoDAL.GetPreTransfer(page, limit, orgId, keyWords);
        }

        public static PagingResult<ArchiveTransferInfo> GetPreReview(int page, int limit, string orgId, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return ArchiveTransferInfoDAL.GetPreReview(page, limit, orgId, keyWords);
        }
        /// <summary>
        /// 导出待审批
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public static List<ArchiveTransferInfo> ExportTransfer(string orgId)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return ArchiveTransferInfoDAL.ExportTransfer(orgId);
        }
        /// <summary>
        /// 导出可移交的
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public static List<ArchiveTransferInfo> ExportTransferList(string orgId, string keywords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return ArchiveTransferInfoDAL.ExportTransferList(orgId, keywords);
        }

        public static bool SubmitReview(ArchiveInfo arcData, UserInfo user)
        {
            if (arcData == null) return false;
            if (string.IsNullOrWhiteSpace(arcData.Id)) return false;
            if (string.IsNullOrWhiteSpace(arcData.LabelCode)) return false;            

            var transferData = new ArchiveTransferInfo()
            {
                Id = Guid.NewGuid().ToString(),
                OrgId = user.OrgId,
                Handover = user.RealName,
                TransferDate = DateTime.Now,
                Status = 0,
                Receiver = "",
                ArchiveId = arcData.Id
            };

            arcData.Status = ArchiveStatusType.草稿;

            return ArchiveTransferInfoDAL.SubmitReview(arcData, transferData) > 0;
        }
        /// <summary>
        /// 判断电子标签是否使用
        /// </summary>
        /// <param name="labelcode"></param>
        /// <returns></returns>
        public static bool CheckLabelCode(string labelcode, string customerno)
        {
            return ArchiveTransferInfoDAL.CheckLabelCode(labelcode, customerno) > 0;
        }

        public static bool RollBack(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            var data = ArchiveTransferInfoDAL.Get(id);
            if (data == null || string.IsNullOrEmpty(data.ArchiveId)) return false;

            return ArchiveTransferInfoDAL.RollBack(data) > 0;
        }

        public static bool PassReview(string id, UserInfo user)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            var data = ArchiveTransferInfoDAL.Get(id);
            if (data == null || string.IsNullOrEmpty(data.ArchiveId)) return false;
            data.Receiver = user.RealName;

            return ArchiveTransferInfoDAL.PassReview(data) > 0;
        }

        public static byte[] Export(string orgId, UserInfo user)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;
            var details = ArchiveTransferInfoDAL.GetInventoryArchiveList(orgId);

            if (details == null || details.Count == 0) return new byte[1];

            StringBuilder sb = new StringBuilder();
            int rownum = 1;
            foreach (var item in details)
            {
                sb.Append(rownum++).Append("|").Append($"{item.LabelCode}".PadRight(18, '0')).Append("|").Append("1".PadLeft(18, '0')).AppendLine();
            }

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
    }
}
