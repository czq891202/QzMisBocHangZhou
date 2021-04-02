using QzMisBocHangZhou.Biz;
using QzMisBocHangZhou.Model;
using System;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class DESKeyController : Controller
    {
        #region【视图控制器】
        /// <summary>
        /// 归还视图
        /// </summary>
        /// <returns></returns>
        public ActionResult DESKeyView()
        {
            return View(new EditViewModel<string>() { Data = "123456", User = AppSession.GetUser() });
        }
        #endregion

        /// <summary>
        /// 保存秘钥
        /// </summary>
        /// <param name="skey"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveKey(string skey)
        {
            string smsg = "";
            try
            {
                if (skey.Length >= 8)
                {
                    DESBiz.SaveKey(skey);
                }
                else
                {
                    smsg = "密钥不能低于8位";
                }
            }
            catch (Exception ex)
            {
                smsg = ex.Message;
            }
            return Json(new { code = 0, Data = skey, msg = smsg });
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="pData">加密数据</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DESEncrypt(string pToEncrypt, string sKey)
        {
            string smsg = "";
            string retB = "";
            try
            {
                if (!string.IsNullOrEmpty(pToEncrypt) && sKey.Length >= 8)
                {
                    retB = DESBiz.DESEncrypt(pToEncrypt, sKey);
                }else if (string.IsNullOrEmpty(pToEncrypt))
                {
                    smsg = "明文不能为空！";
                }
                else
                {
                    smsg = "密钥不能低于8位";
                }
            }
            catch (Exception ex)
            {
                smsg = ex.Message;
            }
            return Json(new { code = 0, data = retB, msg = smsg });
        }
    }
}
