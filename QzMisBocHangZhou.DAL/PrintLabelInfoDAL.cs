using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.DAL
{
    public class PrintLabelInfoDAL
    {

        public static int Edit(PrintLabelInfo info)
        {
            if(IsExits(info.OrgId, info.Year))
            {
                return Update(info);
            }
            else
            {
                return Add(info);
            }
        }

        public static PrintLabelInfo GetInfo(string orgId, int year)
        {
            var sql = @"select p.*, o.Name as OrgName, o.Code as OrgCode from PrintLabelInfo p left join OrgInfo o on p.OrgId = o.Id
                        where p.OrgId = :OrgId and p.Year = :Year";

            return DBCache.DataBase.ExecuteEntity<PrintLabelInfo>(sql,
                DBCache.DataBase.CreatDbParameter("OrgId", orgId),
                DBCache.DataBase.CreatDbParameter("Year", year));
        }

        public static PrintLabelInfo GetInfo(string orgId)
        {
            var sql = @"select p.*, o.Name as OrgName, o.Code as OrgCode from PrintLabelInfo p left join OrgInfo o on p.OrgId = o.Id
                        where p.OrgId = :OrgId";

            return DBCache.DataBase.ExecuteEntity<PrintLabelInfo>(sql,
                DBCache.DataBase.CreatDbParameter("OrgId", orgId));
        }

        private static bool IsExits(string orgId, int Year)
        {
            var sql = @"select count(1) from PrintLabelInfo where OrgId = :OrgId and Year = :Year";

            var ret = DBCache.DataBase.ExecuteScalar<int>(sql,
                DBCache.DataBase.CreatDbParameter("OrgId", orgId),
                DBCache.DataBase.CreatDbParameter("Year", Year));

            return ret > 0;
        }


        private static int Update(PrintLabelInfo info)
        {
            var sql = @"update PrintLabelInfo set MaxNum = MaxNum + :Num where OrgId = :OrgId and Year = :Year";

            return DBCache.DataBase.ExecuteNonQuery(sql,
                DBCache.DataBase.CreatDbParameter("Num", info.MaxNum),
                DBCache.DataBase.CreatDbParameter("OrgId", info.OrgId),
                DBCache.DataBase.CreatDbParameter("Year", info.Year));
        }

        private static int Add(PrintLabelInfo info)
        {
            info.Id = Guid.NewGuid().ToString();
            var sql = @"Insert into PrintLabelInfo (Id, OrgId, Year, MaxNum) values(:Id, :OrgId, :Year, :MaxNum)";

            return DBCache.DataBase.ExecuteNonQuery(sql,
                DBCache.DataBase.CreatDbParameter("Id", info.Id),
                DBCache.DataBase.CreatDbParameter("MaxNum", info.MaxNum),
                DBCache.DataBase.CreatDbParameter("OrgId", info.OrgId),
                DBCache.DataBase.CreatDbParameter("Year", info.Year));
        }
    }
}
