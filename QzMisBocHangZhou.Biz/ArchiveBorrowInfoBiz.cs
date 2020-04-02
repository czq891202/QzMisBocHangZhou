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
    public class ArchiveBorrowInfoBiz
    {
        public static ArchiveBorrowInfo Get(string id)
        {
            return ArchiveBorrowInfoDAL.Get(id);
        }

        public static PagingResult<ArchiveBorrowInfo> GetPreBorrow(int page, int limit, string orgId, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return ArchiveBorrowInfoDAL.GetPreBorrow(page, limit, orgId, keyWords);
        }


        public static PagingResult<ArchiveBorrowInfo> GetPreReview(int page, int limit, string orgId, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return ArchiveBorrowInfoDAL.GetPreReview(page, limit, orgId, keyWords);
        }


        public static PagingResult<ArchiveBorrowInfo> GetPreReturn(int page, int limit, string orgId, string keyWords)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return ArchiveBorrowInfoDAL.GetPreReturn(page, limit, orgId, keyWords);
        }
        

        public static List<ArchiveBorrowInfo> GetExcelData(string orgId)
        {
            if (string.IsNullOrWhiteSpace(orgId)) orgId = OrgInfo.RootId;

            return ArchiveBorrowInfoDAL.GetExcelData(orgId);
        }


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

            return ArchiveBorrowInfoDAL.SubmitReview(data) > 0;
        }


        public static bool RollBack(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            return ArchiveBorrowInfoDAL.RollBack(id) > 0;
        }


        public static bool PassReview(string id, UserInfo user)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            var data = ArchiveBorrowInfoDAL.Get(id);
            if (data == null) return false;
            if (string.IsNullOrWhiteSpace(data.Id)) return false;
            if (string.IsNullOrWhiteSpace(data.ArchiveId)) return false;

            return ArchiveBorrowInfoDAL.PassReview(data) > 0;
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


        public static bool ChangeIn(ArchiveInfo data, string borrowId)
        {
            if (string.IsNullOrWhiteSpace(data.Id)) return false;
            if (string.IsNullOrWhiteSpace(data.LabelCode)) return false;

            var borrowData = ArchiveBorrowInfoDAL.Get(borrowId);

            return ArchiveBorrowInfoDAL.ChangeIn(data, borrowData) > 0;
        }



        //public static ArchiveBorrowInfo Get(string id)
        //{
        //    return ArchiveBorrowInfoDAL.Get(id);
        //}


        //public static PagingResult<ArchiveBorrowInfo> Get(int page, int limit, string orgId)
        //{
        //    if (string.IsNullOrWhiteSpace(orgId) || orgId.Trim() == OrgInfo.RootId)
        //    {
        //        return ArchiveBorrowInfoDAL.Get(page, limit);
        //    }
        //    else
        //    {
        //        return ArchiveBorrowInfoDAL.GetByOrg(page, limit, orgId.Trim());
        //    }
        //}


        //public static PagingResult<ArchiveBorrowInfo> GetApprovaBorrowList(int page, int limit, string orgId)
        //{
        //    if (string.IsNullOrWhiteSpace(orgId) || orgId.Trim() == OrgInfo.RootId)
        //    {
        //        return ArchiveBorrowInfoDAL.GetApprovaBorrowList(page, limit);
        //    }
        //    else
        //    {
        //        return ArchiveBorrowInfoDAL.GetApprovaBorrowList(page, limit, orgId.Trim());
        //    }
        //}


        //public static ArchiveBorrowInfo CreatDefault(string orgId, string user)
        //{
        //    var data = new ArchiveBorrowInfo()
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        OrgId = orgId,
        //        Borrower = user,
        //        BorrowDate = DateTime.Now,
        //        Status = 0,
        //        ArchiveType = 0,
        //    };

        //    ArchiveBorrowInfoDAL.AddInfo(data);
        //    return data;
        //}


        //public static bool SubmitReview(string id)
        //{
        //    if (string.IsNullOrWhiteSpace(id)) return false;

        //    return ArchiveBorrowInfoDAL.SubmitReview(id) > 0;
        //}


        //public static bool Del(string id)
        //{
        //    if (string.IsNullOrWhiteSpace(id)) return false;
        //    ArchiveBorrowInfoDAL.Del(id);
        //    return true;
        //}


        //public static ArchiveBorrowDetails GetDetail(string id)
        //{
        //    if (string.IsNullOrWhiteSpace(id)) return new ArchiveBorrowDetails();
        //    return ArchiveBorrowInfoDAL.GetDetail(id);
        //}

        //public static List<ArchiveBorrowDetails> GetDetails(string pId)
        //{
        //    if (string.IsNullOrWhiteSpace(pId)) return new List<ArchiveBorrowDetails>();
        //    return ArchiveBorrowInfoDAL.GetDetails(pId);
        //}


        //public static PagingResult<SelectArchiveModel> GetArchiveList(int page, int limit, string orgId, string transferId)
        //{
        //    if (string.IsNullOrWhiteSpace(orgId) || orgId.Trim() == OrgInfo.RootId)
        //    {
        //        return ArchiveBorrowInfoDAL.GetArchiveList(page, limit, transferId.Trim());
        //    }
        //    else
        //    {
        //        return ArchiveBorrowInfoDAL.GetArchiveList(page, limit, orgId.Trim(), transferId.Trim());
        //    }
        //}


        //public static bool AppendArchive(string tId, string usedBy, List<SelectArchiveModel> arcList)
        //{
        //    if (string.IsNullOrWhiteSpace(tId)) return false;
        //    if (arcList == null || arcList.Count == 0) return false;

        //    if (string.IsNullOrWhiteSpace(usedBy)) usedBy = "期转现";
        //    tId = tId.Trim();
        //    usedBy = usedBy.Trim();

        //    var info = ArchiveBorrowInfoDAL.Get(tId);
        //    if (info == null || string.IsNullOrWhiteSpace(info.Id)) return false;

        //    var day = BaseDataDAL.GetDay(usedBy);
        //    if (day == 0) day = 15;

        //    var date = info.BorrowDate.HasValue ? info.BorrowDate.Value.AddDays(day) : DateTime.Now.AddDays(day);

        //    var data = new List<ArchiveBorrowDetails>();
        //    foreach(var item in arcList)
        //    {
        //        data.Add(new ArchiveBorrowDetails()
        //        {
        //            Id = Guid.NewGuid().ToString(),
        //            PId = tId,
        //            ArchiveId = item.Id,
        //            UsedBy = usedBy,
        //            PreReturnDate = date
        //        });
        //    }

        //    return ArchiveBorrowInfoDAL.AddDetails(data) > 0;
        //}


        //public static bool UpdateInfo(ArchiveBorrowInfo info, UserInfo user)
        //{
        //    if (info == null || user == null) return false;
        //    if (string.IsNullOrWhiteSpace(info.Id)) return false;
        //    if (!info.BorrowDate.HasValue) return false;

        //    if(string.IsNullOrWhiteSpace(info.OrgId)) info.OrgId = user.OrgId;
        //    info.Borrower = user.RealName;

        //    return ArchiveBorrowInfoDAL.UpdateInfo(info) > 0;

        //}


        //public static bool DelDetails(string id)
        //{
        //    if (string.IsNullOrWhiteSpace(id)) return false;

        //    return ArchiveBorrowInfoDAL.DelDetail(id) > 0;
        //}





        //public static bool RollBack(string id)
        //{
        //    if (string.IsNullOrWhiteSpace(id)) return false;
        //    return ArchiveBorrowInfoDAL.RollBack(id) > 0;
        //}

        //public static bool ReturnArchive(ArchiveBorrowDetails details)
        //{
        //    if (details == null) return false;
        //    if (string.IsNullOrWhiteSpace(details.Id)) return false;
        //    if (string.IsNullOrWhiteSpace(details.PId)) return false;
        //    if (string.IsNullOrWhiteSpace(details.ArchiveId)) return false;
        //    if (string.IsNullOrWhiteSpace(details.ReturnPepole)) return false;
        //    if (!details.RealReturnDate.HasValue) details.RealReturnDate = DateTime.Now;

        //    return ArchiveBorrowInfoDAL.ReturnArchive(details) > 0;
        //}

        //public static byte[] Export(string pId)
        //{
        //    var data = ArchiveBorrowInfoDAL.GetBorrowInventoryDetails(pId);

        //    StringBuilder sb = new StringBuilder();
        //    foreach (var item in data)
        //    {
        //        sb.Append("|".PadRight(12, '|')).Append($"{item.LabelCode}".PadRight(18, '0')).Append("|").Append("1".PadLeft(18, '0')).Append("|".PadRight(5, '|')).AppendLine();
        //    }

        //    return Encoding.UTF8.GetBytes(sb.ToString());
        //}


        //public static bool VerifyData(string id, string fileName)
        //{
        //    var data = File.ReadAllLines(fileName);

        //    var fullDetails = ArchiveBorrowInfoDAL.GetDetails(id);

        //    foreach (var item in data)
        //    {
        //        if (string.IsNullOrWhiteSpace(item) || item.Length < 18) continue;
        //        var detailInfo = fullDetails.Find(p => !string.IsNullOrWhiteSpace(p.LabelCode) && p.LabelCode.PadRight(18, '0') == item.Substring(0, 18));
        //        if (detailInfo == null) continue;
        //        Enum.TryParse(item.Substring(41, 1), out VerifyType type);
        //        detailInfo.Status = type;
        //    }

        //    var details = fullDetails.FindAll(p => p.Status == VerifyType.成功);
        //    ArchiveBorrowInfoDAL.VerifyData(details);

        //    return true;
        //}
    }
}
