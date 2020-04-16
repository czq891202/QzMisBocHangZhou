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

        public static List<ArchiveTransferInfo> GetExcelData(string orgId)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return ArchiveTransferInfoDAL.GetExcelData(orgId);
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

        public static bool RollBack(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            return ArchiveTransferInfoDAL.RollBack(id) > 0;
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
            var info = CreatNewInfo(user);
            var details = ArchiveTransferInfoDAL.GetInventoryArchiveList(orgId);

            if (details == null || details.Count == 0) return new byte[1];

            StringBuilder sb = new StringBuilder();
            foreach (var item in details)
            {
                item.Id = Guid.NewGuid().ToString();
                item.InventoryId = info.Id;
                item.Status = VerifyType.未核对;

                sb.Append("|".PadRight(12, '|')).Append($"{item.LabelCode}".PadRight(18, '0')).Append("|").Append("1".PadLeft(18, '0')).Append("|".PadRight(5, '|')).AppendLine();
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

        //public static PagingResult<ArchiveTransferInfo> GetApprovaTransferList(int page, int limit, string orgId)
        //{
        //    if (string.IsNullOrWhiteSpace(orgId) || orgId.Trim() == OrgInfo.RootId)
        //    {
        //        return ArchiveTransferInfoDAL.GetApprovaTransferList(page, limit);
        //    }
        //    else
        //    {
        //        return ArchiveTransferInfoDAL.GetApprovaTransferList(page, limit, orgId.Trim());
        //    }
        //}

        //public static ArchiveTransferInfo Get(string id)
        //{
        //    return ArchiveTransferInfoDAL.Get(id);
        //}

        //public static PagingResult<ArchiveTransferInfo> Get(int page, int limit, string orgId)
        //{
        //    if (string.IsNullOrWhiteSpace(orgId) || orgId.Trim() == OrgInfo.RootId)
        //    {
        //        return ArchiveTransferInfoDAL.Get(page, limit);
        //    }
        //    else
        //    {
        //        return ArchiveTransferInfoDAL.GetByOrg(page, limit, orgId.Trim());
        //    }
        //}


        //public static ArchiveTransferInfo CreatDefault(string orgId, string user)
        //{
        //    var data = new ArchiveTransferInfo()
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        OrgId = orgId,
        //        Handover = user,
        //        TransferDate = DateTime.Now,
        //        Status = 0,
        //        Receiver = "",
        //    };

        //    ArchiveTransferInfoDAL.Add(data, null);
        //    return data;
        //}


        //public static bool Update(ArchiveTransferInfo data)
        //{
        //    if (string.IsNullOrWhiteSpace(data.Id)) return false;

        //    return ArchiveTransferInfoDAL.Update(data, null) > 0;
        //}


        //public static bool EditDetails(ArchiveTransferDetails data)
        //{
        //    if (string.IsNullOrWhiteSpace(data.PId) || string.IsNullOrWhiteSpace(data.ArchiveId)) return false;
        //    return ArchiveTransferInfoDAL.EditDetails(data) > 0;
        //}


        //public static ArchiveTransferDetails GetDetails(string pId, string archiveId)
        //{
        //    if (string.IsNullOrWhiteSpace(pId) || string.IsNullOrWhiteSpace(archiveId)) return new ArchiveTransferDetails();
        //    return ArchiveTransferInfoDAL.GetDetails(pId, archiveId);
        //}


        //public static List<ArchiveTransferDetails> GetDetails(string pId)
        //{
        //    if (string.IsNullOrWhiteSpace(pId)) return new List<ArchiveTransferDetails>();
        //    return ArchiveTransferInfoDAL.GetDetails(pId);
        //}


        //public static PagingResult<SelectArchiveModel> GetArchiveList(int page, int limit, string orgId, string transferId)
        //{
        //    if (string.IsNullOrWhiteSpace(orgId) || orgId.Trim() == OrgInfo.RootId)
        //    {
        //        return ArchiveTransferInfoDAL.GetArchiveList(page, limit, transferId.Trim());
        //    }
        //    else
        //    {
        //        return ArchiveTransferInfoDAL.GetArchiveList(page, limit, orgId.Trim(), transferId.Trim());
        //    }
        //}


        //public static bool AddTransferDetail(string transferId, string archiveId)
        //{
        //    if (string.IsNullOrWhiteSpace(transferId) || string.IsNullOrWhiteSpace(archiveId)) return false;

        //    var data = new ArchiveTransferDetails()
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        ArchiveId = archiveId,
        //        PId = transferId
        //    };

        //    return ArchiveTransferInfoDAL.Add(null, new List<ArchiveTransferDetails>() { data }) > 0;
        //}

        //public static bool DelTransferDetail(string transferId, string archiveId)
        //{
        //    if (string.IsNullOrWhiteSpace(transferId) || string.IsNullOrWhiteSpace(archiveId)) return false;
        //    ArchiveTransferInfoDAL.DelDetail(transferId, archiveId);
        //    return true;
        //}

        //public static bool Del(string id)
        //{
        //    if (string.IsNullOrWhiteSpace(id)) return false;
        //    ArchiveTransferInfoDAL.Del(id);
        //    return true;
        //}

        //public static bool RollBack(string id)
        //{
        //    if (string.IsNullOrWhiteSpace(id)) return false;
        //    return ArchiveTransferInfoDAL.RollBack(id) > 0;
        //}


        //public static byte[] Export(string pId)
        //{
        //    var data = ArchiveTransferInfoDAL.GetTransferInventoryDetails(pId);

        //    StringBuilder sb = new StringBuilder();
        //    foreach (var item in data)
        //    {
        //        sb.Append("|".PadRight(12, '|')).Append($"{item.LabelCode}".PadRight(18, '0')).Append("|").Append("1".PadLeft(18, '0')).Append("|".PadRight(5, '|')).AppendLine();
        //    }

        //    return Encoding.UTF8.GetBytes(sb.ToString());
        //}


        //public static bool VerifyData(string id, string fileName, UserInfo user)
        //{
        //    //var data = File.ReadAllLines(fileName);

        //    //var info = ArchiveTransferInfoDAL.Get(id);
        //    //var fullDetails = ArchiveTransferInfoDAL.GetDetails(id);

        //    //foreach (var item in data)
        //    //{
        //    //    if (string.IsNullOrWhiteSpace(item) || item.Length < 18) continue;
        //    //    var detailInfo = fullDetails.Find(p => !string.IsNullOrWhiteSpace(p.LabelCode) && p.LabelCode.PadRight(18, '0') == item.Substring(0, 18));
        //    //    if (detailInfo == null) continue;
        //    //    Enum.TryParse(item.Substring(41, 1), out VerifyType type);
        //    //    detailInfo.Status = type;
        //    //}

        //    //var details = fullDetails.FindAll(p => p.Status == VerifyType.成功);
        //    //ArchiveTransferInfoDAL.VerifyData(details);


        //    //if (details != null && fullDetails.Count == details.Count)
        //    //{
        //    //    info.Status = 2;
        //    //    info.Receiver = user.RealName;
        //    //    ArchiveTransferInfoDAL.PassReview(info);
        //    //}

        //    return true;
        //}
    }

}
