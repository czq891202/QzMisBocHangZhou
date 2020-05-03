using QzMisBocHangZhou.Biz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class ReportController : Controller
    {
        #region 【视图】
        public ActionResult BorrowTimeOutView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");

            return View(AppSession.GetUser());
        }


        public ActionResult TransferTimeOutView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");

            return View(AppSession.GetUser());
        }

        public ActionResult SettleTimeOutView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");

            return View(AppSession.GetUser());
        }

        public ActionResult ArchiveInfoTimeView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");

            return View(AppSession.GetUser());
        }

        #endregion

        #region 【接口】
        /// <summary>
        /// 借阅超时
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="guaranteeType"></param>
        /// <param name="status"></param>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetBorrowTimeOut(string orgId, string guaranteeType, string status, string keyWords)
        {
            var result = ReportBiz.GetBorrowTimeOut(orgId, guaranteeType, keyWords);
            return Json(new { code = 0, data = result, msg = "" });
        }
        /// <summary>
        /// 移交超时
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="guaranteeType"></param>
        /// <param name="status"></param>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetTransferTimeOut(string orgId, string guaranteeType, string status, string keyWords)
        {
            var result = ReportBiz.GetTransferTimeOut(orgId, guaranteeType, keyWords);
            return Json(new { code = 0, data = result, msg = "" });
        }
        /// <summary>
        /// 结清超时
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="guaranteeType"></param>
        /// <param name="status"></param>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSettleTimeOut(string orgId, string guaranteeType, string status, string keyWords)
        {
            var result = ReportBiz.GetSettleTimeOut(orgId, guaranteeType, keyWords);
            return Json(new { code = 0, data = result, msg = "" });
        }
        /// <summary>
        /// 档案追溯
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="guaranteeType"></param>
        /// <param name="status"></param>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetArchiveInfoTime(int page, int limit, string orgId, string guaranteeType, string status, string keyWords)
        {
            var data = ReportBiz.GetArchiveInfoTime(page, limit, orgId, guaranteeType, status, keyWords);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }
        #endregion
    }
}