using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.DAL
{
    public class BaseDataDAL
    {
        public static List<ArchiveProcTime> GetArchiveProcTimeList()
        {
            var sql = "select * from ArchiveProcTime order by id";
            return DBCache.DataBase.ExecuteEntityList<ArchiveProcTime>(sql);
        }

        public static int UpDateArchiveProcTime(ArchiveProcTime data)
        {
            var sql = @"update ArchiveProcTime set Day = :Day where Id = :Id";

            return DBCache.DataBase.ExecuteNonQuery(sql,
                DBCache.DataBase.CreatDbParameter("Day", data.Day),
                DBCache.DataBase.CreatDbParameter("Id", data.Id));
        }

        public static int GetDay(string type)
        {
            var sql = @"select Day from  ArchiveProcTime where ProcType = :ProcType";

            return DBCache.DataBase.ExecuteScalar<int>(sql,
                DBCache.DataBase.CreatDbParameter("ProcType", type));
        }
    }
}
