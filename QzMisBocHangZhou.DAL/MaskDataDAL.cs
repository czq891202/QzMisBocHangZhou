using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.DAL
{
    public class MaskDataDAL
    {
        public static PagingResult<MaskData> Get(int page, int limit, string keyWords)
        {
            var sql = $"select * from MaskData where data like '%{keyWords}%' order by Type, data";

            var rCount = DBCache.DataBase.GetRecordCount(sql);
            var data = DBCache.DataBase.ExecuteEntityListByPageing<MaskData>(page, limit, sql);

            return new PagingResult<MaskData>() { Count = rCount, Result = data };
        }

        public static int Add(List<MaskData> data)
        {
            var ret = 0;
            foreach(var item in data)
            {
                DBCache.DataBase.ExecuteNonQuery((cmd) =>
                {
                    Add(cmd, item);
                    DelArcBySeq(cmd, item);
                    return 1;
                });

                ret++;
            }

            return ret;
        }

        private static int Add(DbCommand cmd, MaskData data)
        {
            var sql = @"insert into MaskData (Id, Data, Type) values(:Id, :Data, :Type)";
            cmd.CommandText = sql;

            cmd.Parameters.Clear();
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Id", data.Id));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Data", data.Data));
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("Type", data.Type));

            return cmd.ExecuteNonQuery();
        }

        private static int DelArcBySeq(DbCommand cmd, MaskData data)
        {
            var sql = @"delete ArchiveInfo where SeqNo = :SeqNo";
            cmd.CommandText = sql;

            cmd.Parameters.Clear();
            cmd.Parameters.Add(DBCache.DataBase.CreatDbParameter("SeqNo", data.Data));
            return cmd.ExecuteNonQuery();
        }

        public static int Del(string id)
        {
            var sql = "delete from MaskData where Id = :Id";
            return DBCache.DataBase.ExecuteNonQuery(sql, DBCache.DataBase.CreatDbParameter("Id", id));
        }
    }
}
