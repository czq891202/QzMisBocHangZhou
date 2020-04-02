using QzMisBocHangZhou.DAL;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Biz
{
    public class BaseDataBiz
    {
        public static List<ArchiveProcTime> GetArchiveProcTimeList()
        {
            return BaseDataDAL.GetArchiveProcTimeList();
        }

        public static bool UpDateArchiveProcTime(ArchiveProcTime data)
        {
            if (data == null) return false;
            if (string.IsNullOrWhiteSpace(data.Id)) return false;
            if (string.IsNullOrWhiteSpace(data.ProcType)) return false;
            if (data.Day < 1) return false;

            return BaseDataDAL.UpDateArchiveProcTime(data) > 0;
        }
    }
}
