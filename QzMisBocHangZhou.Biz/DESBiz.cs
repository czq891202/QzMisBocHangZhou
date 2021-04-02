using QzMisBocHangZhou.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Biz
{
    public class DESBiz
    {
        public static void SaveKey(string skey)
        {
            DESHelper.SetKey(skey);
        }

        public static string DESEncrypt(string pToEncrypt, string key = "")
        {
            return DESHelper.DESEncrypt(pToEncrypt, key);
        }
    }
}
