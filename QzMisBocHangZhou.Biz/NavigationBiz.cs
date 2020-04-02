using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QzMisBocHangZhou.DAL;

namespace QzMisBocHangZhou.Biz
{
    public class NavigationBiz
    {
        public static List<Navigation> Get()
        {
            return NavigationInfoDAL.Get();
        }


        public static Navigation Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return new Navigation();

            return NavigationInfoDAL.Get(id);
        }


        public static List<Navigation> GetByRole(string roleId)
        {
            if (string.IsNullOrWhiteSpace(roleId)) return new List<Navigation>();

            return NavigationInfoDAL.GetByRole(roleId);
        }



        public static bool Add(Navigation data)
        {
            data.Id = Guid.NewGuid().ToString();
            if (!RequiredData(data)) return false;

            return NavigationInfoDAL.Add(data) > 0;
        }

        public static bool Update(Navigation data)
        {
            if (!RequiredData(data)) return false;
            return NavigationInfoDAL.Update(data) > 0;
        }


        public static bool Del(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            return NavigationInfoDAL.Del(id) >= 0;
        }


        private static bool RequiredData(Navigation data)
        {
            if (data == null) return false;
            if (string.IsNullOrWhiteSpace(data.Id)) return false;
            if (string.IsNullOrWhiteSpace(data.ParentId)) return false;
            if (string.IsNullOrWhiteSpace(data.Name)) return false;
            if (string.IsNullOrWhiteSpace(data.Title)) return false;

            return true;
        }
    }
}
