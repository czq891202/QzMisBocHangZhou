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
        
        public static List<OrgTree> GetOrgTree(string orgids)
        {
            List<OrgTree> trees = new List<OrgTree>();
            if (string.IsNullOrEmpty(orgids))
            {
                orgids = "00000";
            }
            foreach (string orgid in orgids.Split(','))
            {
                var orgInfo = OrgInfoDAL.GetChild(orgid);
                if (orgInfo != null)
                {
                    OrgTree treep = new OrgTree();
                    var org = orgInfo.First(r => r.Id == orgid);
                    treep.id = org?.Id;
                    treep.name = org?.Name;
                    treep.children = new List<OrgTree>();
                    trees.Add(treep);
                    SetOrgTree(treep.children, orgid, orgInfo);
                }
            }
            return trees;
        }

        private static void SetOrgTree(List<OrgTree> trees,string parentid,List<OrgInfo> orgs)
        {
            var drs = orgs.Where(r => r.ParentId == parentid).ToList();
            foreach(var dr in drs)
            {
                OrgTree tree = new OrgTree();
                tree.id = dr.Id;
                tree.parentId = dr.ParentId;
                tree.name = dr.Name;
                trees.Add(tree);
                tree.children = new List<OrgTree>();
                SetOrgTree(tree.children, dr.Id, orgs);
            }
        }
        
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
