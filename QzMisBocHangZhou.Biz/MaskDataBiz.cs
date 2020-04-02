using QzMisBocHangZhou.DAL;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Biz
{
    public class MaskDataBiz
    {
        public static PagingResult<MaskData> Get(int page, int limit)
        {
            return MaskDataDAL.Get(page, limit);
        }

        public static bool Add(List<string> inputData)
        {
            var data = PreDataHandel(inputData);
            if (data == null || data.Count == 0) return true;

            return MaskDataDAL.Add(data) > 0;
        }

        private static List<MaskData> PreDataHandel(List<string> inputData)
        {
            var data = new List<MaskData>();

            if (inputData == null || inputData.Count == 0) return data;

            foreach (var item in inputData)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;
                data.Add(new MaskData()
                {
                    Id = Guid.NewGuid().ToString(),
                    Data = item,
                    Type = 0
                });
            }

            return data;
        }


        public static bool Del(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return true;

            MaskDataDAL.Del(id);
            return true;
        }
    }
}
