using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QzMisBocHangZhou.Model;
using QzMisBocHangZhou.DAL;

namespace QzMisBocHangZhou.Biz
{
    public class OrgInfoBiz
    {
        public static List<OrgInfo> GetAll()
        {
            return OrgInfoDAL.GetAll();
        }


        public static OrgInfo Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return new OrgInfo();
            return OrgInfoDAL.Get(id);
        }


        public static OrgInfo GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return new OrgInfo();
            return OrgInfoDAL.GetByCode(code);
        }

        public static List<OrgInfo> GetChild(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return new List<OrgInfo>();
            return OrgInfoDAL.GetChild(id);
        }


        public static bool Add(OrgInfo data)
        {
            data.Id = Guid.NewGuid().ToString();
            if (!RequiredData(data)) return false;
            return OrgInfoDAL.Add(data) > 0;
        }


        public static bool Update(OrgInfo data)
        {
            if (!RequiredData(data)) return false;
            return OrgInfoDAL.Update(data) > 0;
        }


        //public static bool Disable(string id)
        //{
        //    if (string.IsNullOrWhiteSpace(id)) return false;
        //    return OrgInfoDAL.Disable(id) > 0;
        //}


        //public static bool Enable(string id)
        //{
        //    if (string.IsNullOrWhiteSpace(id)) return false;
        //    return OrgInfoDAL.Enable(id) > 0;
        //}

        private static bool RequiredData(OrgInfo data)
        {
            if (data == null) return false;
            if (string.IsNullOrWhiteSpace(data.Id)) return false;
            if (string.IsNullOrWhiteSpace(data.ParentId)) return false;
            if (string.IsNullOrWhiteSpace(data.Name)) return false;
            if (string.IsNullOrWhiteSpace(data.TypeName)) return false;
            if (string.IsNullOrWhiteSpace(data.ShortName)) return false;

            return true;
        }
    }
}
