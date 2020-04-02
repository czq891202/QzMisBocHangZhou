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
        public JsonResult GetBorrowTimeOut(string orgId)
        {
            var result = ReportBiz.GetBorrowTimeOut(orgId);
            return Json(new { code = 0, data = result, msg = "" });
        }


        [HttpPost]
        public JsonResult GetTransferTimeOut(string orgId)
        {
            var result = ReportBiz.GetTransferTimeOut(orgId);
            return Json(new { code = 0, data = result, msg = "" });
        }


        [HttpPost]
        public JsonResult GetSettleTimeOut(string orgId)
        {
            var result = ReportBiz.GetSettleTimeOut(orgId);
            return Json(new { code = 0, data = result, msg = "" });
        }

        #endregion


        //#region 【视图】
        //public ActionResult BorrowTimeOutChartView()
        //{
        //    if (!AppSession.IsExits()) return Redirect("/Login/LoginView");
        //    return View();
        //}


        //public ActionResult BorrowTimeOutListView()
        //{
        //    if (!AppSession.IsExits()) return Redirect("/Login/LoginView");

        //    return View();
        //}


        //public ActionResult ArchiveChartView()
        //{
        //    if (!AppSession.IsExits()) return Redirect("/Login/LoginView");

        //    return View();
        //}

        //#endregion


        //[HttpPost]
        //public JsonResult GetByYearMonth(DateTime? yearMonth)
        //{
        //    if (!yearMonth.HasValue) yearMonth = DateTime.Now;
        //    var result = ReportBiz.GetByYearMonth(yearMonth.Value);
        //    return Json(new { code = 0, data = result, msg = "" });
        //}

        //[HttpPost]
        //public JsonResult GetByDay(DateTime? day)
        //{
        //    if (!day.HasValue) day = DateTime.Now;
        //    var result = ReportBiz.GetByDay(day.Value);
        //    return Json(new { code = 0, data = result, msg = "" });
        //}


        //[HttpPost]
        //public JsonResult GetArchiveTotal()
        //{
        //    var result = ReportBiz.GetArchiveTotal();
        //    return Json(new { code = 0, data = result, msg = "" });
        //}
    }
}