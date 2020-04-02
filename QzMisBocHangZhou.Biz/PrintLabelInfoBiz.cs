using QzMisBocHangZhou.DAL;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Biz
{
    public class PrintLabelInfoBiz
    {
        public static bool Edit(PrintLabelInfo info)
        {
            if (info == null) return false;
            if (string.IsNullOrWhiteSpace(info.OrgId)) return false;
            if (info.MaxNum == 0) return false;
            return PrintLabelInfoDAL.Edit(info) > 0;
        }

        public static PrintLabelInfo GetInfo(string orgId, int year)
        {
            if (string.IsNullOrWhiteSpace(orgId)) throw new Exception("error");

            var result = PrintLabelInfoDAL.GetInfo(orgId, year);

            if(result == null || string.IsNullOrWhiteSpace(result.Id))
            {
                Edit(new PrintLabelInfo() { OrgId = orgId, Year = year, MaxNum = 1 });

                result = PrintLabelInfoDAL.GetInfo(orgId, year);
            }

            return result;
        }


        public static PrintLabelInfo GetPrintInfo(string labelNo)
        {
            var info = new PrintLabelInfo();

            info.Year = int.Parse(labelNo.Substring(1, 4));
            info.OrgCode = labelNo.Substring(5, 5);
            info.MaxNum = int.Parse(labelNo.Substring(10, 5));

            var orgInfo = OrgInfoBiz.GetByCode(info.OrgCode);
            if (orgInfo == null) throw new Exception("机构代码错误!");
            info.OrgName = orgInfo.Name;
            info.OrgId = orgInfo.Id;

            return info;
        }
    }
}
