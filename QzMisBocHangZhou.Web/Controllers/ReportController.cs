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

        #endregion

        #region 【接口】
        [HttpPost]
        public JsonResult GetBorrowTimeOut(string orgId, string guaranteeType, string status, string keyWords)
        {
            var result = ReportBiz.GetBorrowTimeOut(orgId, guaranteeType, keyWords);
            return Json(new { code = 0, data = result, msg = "" });
        }


        [HttpPost]
        public JsonResult GetTransferTimeOut(string orgId, string guaranteeType, string status, string keyWords)
        {
            var result = ReportBiz.GetTransferTimeOut(orgId, guaranteeType, keyWords);
            return Json(new { code = 0, data = result, msg = "" });
        }


        [HttpPost]
        public JsonResult GetSettleTimeOut(string orgId, string guaranteeType, string status, string keyWords)
        {
            var result = ReportBiz.GetSettleTimeOut(orgId, guaranteeType, keyWords);
            return Json(new { code = 0, data = result, msg = "" });
        }

        #endregion
    }
}