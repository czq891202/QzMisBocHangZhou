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

            return ArchiveSettleInfoDAL.RollBack(id) > 0;
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



        //public static PagingResult<ArchiveSettleInfo> GetApprovaSettleList(int page, int limit, string orgId)
        //{
        //    if (string.IsNullOrWhiteSpace(orgId) || orgId.Trim() == OrgInfo.RootId)
        //    {
        //        return ArchiveSettleInfoDAL.GetApprovaSettleList(page, limit);
        //    }
        //    else
        //    {
        //        return ArchiveSettleInfoDAL.GetApprovaSettleList(page, limit, orgId.Trim());
        //    }
        //}

        //public static ArchiveSettleInfo Get(string id)
        //{
        //    return ArchiveSettleInfoDAL.Get(id);
        //}

        //public static PagingResult<ArchiveSettleInfo> Get(int page, int limit, string orgId)
        //{
        //    if (string.IsNullOrWhiteSpace(orgId) || orgId.Trim() == OrgInfo.RootId)
        //    {
        //        return ArchiveSettleInfoDAL.Get(page, limit);
        //    }
        //    else
        //    {
        //        return ArchiveSettleInfoDAL.GetByOrg(page, limit, orgId.Trim());
        //    }
        //}


        //public static ArchiveSettleInfo CreatDefault(string orgId, string user)
        //{
        //    var data = new ArchiveSettleInfo()
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        OrgId = orgId,
        //        SettlePeople = user,
        //        SettleDate = DateTime.Now,
        //        Status = 0,
        //    };

        //    ArchiveSettleInfoDAL.Add(data, null);
        //    return data;
        //}


        //public static bool Update(ArchiveSettleInfo data)
        //{
        //    if (string.IsNullOrWhiteSpace(data.Id)) return false;

        //    return ArchiveSettleInfoDAL.Update(data, null) > 0;
        //}


        //public static bool EditDetails(ArchiveSettleDetail data)
        //{
        //    if (string.IsNullOrWhiteSpace(data.PId) || string.IsNullOrWhiteSpace(data.ArchiveId)) return false;
        //    return ArchiveSettleInfoDAL.EditDetails(data) > 0;
        //}


        //public static ArchiveSettleDetail GetDetails(string pId, string archiveId)
        //{
        //    if (string.IsNullOrWhiteSpace(pId) || string.IsNullOrWhiteSpace(archiveId)) return new ArchiveSettleDetail();
        //    return ArchiveSettleInfoDAL.GetDetails(pId, archiveId);
        //}


        //public static List<ArchiveSettleDetail> GetDetails(string pId)
        //{
        //    if (string.IsNullOrWhiteSpace(pId)) return new List<ArchiveSettleDetail>();
        //    return ArchiveSettleInfoDAL.GetDetails(pId);
        //}


        //public static PagingResult<SelectArchiveModel> GetArchiveList(int page, int limit, string orgId, string settleId)
        //{
        //    if (string.IsNullOrWhiteSpace(orgId) || orgId.Trim() == OrgInfo.RootId)
        //    {
        //        return ArchiveSettleInfoDAL.GetArchiveList(page, limit, settleId.Trim());
        //    }
        //    else
        //    {
        //        return ArchiveSettleInfoDAL.GetArchiveList(page, limit, orgId.Trim(), settleId.Trim());
        //    }
        //}


        //public static bool AddSettleDetail(string settleId, string archiveId)
        //{
        //    if (string.IsNullOrWhiteSpace(settleId) || string.IsNullOrWhiteSpace(archiveId)) return false;

        //    var data = new ArchiveSettleDetail()
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        ArchiveId = archiveId,
        //        PId = settleId
        //    };

        //    return ArchiveSettleInfoDAL.Add(null, new List<ArchiveSettleDetail>() { data }) > 0;
        //}

        //public static bool DelSettleDetail(string settleId, string archiveId)
        //{
        //    if (string.IsNullOrWhiteSpace(settleId) || string.IsNullOrWhiteSpace(archiveId)) return false;
        //    ArchiveSettleInfoDAL.DelDetail(settleId, archiveId);
        //    return true;
        //}

        //public static bool Del(string id)
        //{
        //    if (string.IsNullOrWhiteSpace(id)) return false;
        //    ArchiveSettleInfoDAL.Del(id);
        //    return true;
        //}

        //public static bool SubmitReview(string id)
        //{
        //    if (string.IsNullOrWhiteSpace(id)) return false;

        //    return ArchiveSettleInfoDAL.SubmitReview(id) > 0;
        //}

        //public static bool PassReview(string id, UserInfo user)
        //{
        //    if (string.IsNullOrWhiteSpace(id)) return false;
        //    var data = ArchiveSettleInfoDAL.Get(id);
        //    if (data == null || string.IsNullOrEmpty(data.Id)) return false;

        //    return ArchiveSettleInfoDAL.PassReview(data) > 0;
        //}


        //public static bool RollBack(string id)
        //{
        //    if (string.IsNullOrWhiteSpace(id)) return false;
        //    return ArchiveSettleInfoDAL.RollBack(id) > 0;
        //}
    }
}
