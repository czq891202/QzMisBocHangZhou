using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.DAL
{
    public class DESHelper
    {
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="pToEncrypt">加密数据</param>
        /// <param name="sKey">8位字符的密钥字符串</param>
        /// <returns></returns>
        public static string DESEncrypt(string pToEncrypt, string sKey = "")
        {
            if (string.IsNullOrEmpty(sKey))
            {
                sKey = GetKey();
            }
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);// 密匙
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);// 初始化向量
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var retB = Convert.ToBase64String(ms.ToArray());
            return retB;
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="pToDecrypt">解密数据</param>
        /// <param name="sKey">8位字符的密钥字符串(需要和加密时相同)</param>
        /// <returns></returns>
        public static string DESDecrypt(string pToDecrypt, string sKey = "")
        {
            if(string.IsNullOrEmpty(sKey))
            {
                sKey = GetKey();
            }
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            // 如果两次密匙不一样，这一步可能会引发异常
            cs.FlushFinalBlock();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        /// <summary>
        /// 获取Key
        /// </summary>
        /// <returns></returns>
        public static string GetKey()
        {
            string sKey = string.Empty;
            using (FileStream fs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory +"/DESKey/DESKey.txt", FileMode.Open))
            {
                //定义存放文件信息的字节数组
                byte[] bytes = new byte[fs.Length];
                //读取文件信息
                fs.Read(bytes, 0, bytes.Length);
                //将得到的字节型数组重写编码为string类型
                sKey = Encoding.UTF8.GetString(bytes);
            }
            return sKey;
        }
        /// <summary>
        /// 设置Key
        /// </summary>
        /// <param name="sKey"></param>
        public static void SetKey(string sKey)
        {
            if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "/DESKey/DESKey.txt"))
                File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + "/DESKey/DESKey.txt");
            using (FileStream fs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "/DESKey/DESKey.txt", FileMode.OpenOrCreate))
            {
                //将字符串转换为字节数组
                byte[] bytes = Encoding.UTF8.GetBytes(sKey);
                //向文件中写入字节数组
                fs.Write(bytes, 0, bytes.Length);
                //刷新缓冲区
                fs.Flush();
                //关闭流
                fs.Close();
            }
        }
    }
}
