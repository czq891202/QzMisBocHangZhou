using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QzMisBocHangZhou.Biz;
using System.IO;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class SettleController : Controller
    {
        #region 【视图控制器】
        /// <summary>
        /// 结清清单
        /// </summary>
        /// <returns></returns>
        public ActionResult ListView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");
            return View(AppSession.GetUser());
        }


        public ActionResult AppendArchiveView(string tId)
        {
            return View(new EditViewModel<string>() { Data = tId, User = AppSession.GetUser() });
        }


        /// <summary>
        /// 审批列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ApprovalListView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");
            return View(AppSession.GetUser());
        }

        ///// <summary>
        ///// 结清编辑
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult EditView(string mode, string tId)
        //{
        //    if (string.IsNullOrWhiteSpace(mode) || mode.Equals("add", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(tId))
        //    {
        //        var user = AppSession.GetUser();
        //        var data = ArchiveSettleInfoBiz.CreatDefault(user.OrgId, user.RealName);
        //        tId = data.Id;
        //        //return View(new EditViewModel<ArchiveSettleInfo>() { Data = result, User = AppSession.GetUser() });
        //    }

        //    var result = ArchiveSettleInfoBiz.Get(tId);
        //    return View(new EditViewModel<ArchiveSettleInfo>() { Data = result, User = AppSession.GetUser() });
        //}


        ///// <summary>
        ///// 结清查看
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult ShowView(string tId)
        //{
        //    var result = ArchiveSettleInfoBiz.Get(tId);
        //    return View(result);
        //}



        ///// <summary>
        ///// 结清审批列表
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult ApprovalListView()
        //{
        //    if (!AppSession.IsExits()) return Redirect("/Login/LoginView");
        //    return View(AppSession.GetUser());
        //}



        ///// <summary>
        ///// 结清审批
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult ApprovalView(string tId)
        //{
        //    var result = ArchiveSettleInfoBiz.Get(tId);
        //    return View(result);
        //}

        #endregion


        #region 接口
        [HttpPost]
        public JsonResult GetPreSettle(int page, int limit, string orgId, string keywords)
        {
            var data = ArchiveSettleInfoBiz.GetPreSettle(page, limit, orgId, keywords);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }

        [HttpPost]
        public JsonResult GetPreReview(int page, int limit, string orgId, string keywords)
        {
            var data = ArchiveSettleInfoBiz.GetPreReview(page, limit, orgId, keywords);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }

        [HttpPost]
        public JsonResult GetPreOut(int page, int limit, string orgId, string keywords)
        {
            var data = ArchiveSettleInfoBiz.GetPreOut(page, limit, orgId, keywords);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }


        [HttpPost]
        public JsonResult SubmitReview(string tId, string usedBy, DateTime? settleDate)
        {
            var success = ArchiveSettleInfoBiz.SubmitReview(tId, usedBy, settleDate, AppSession.GetUser());
            return Json(new { code = 0, data = success, msg = "" });
        }


        [HttpPost]
        public JsonResult RollBack(string id)
        {
            var success = ArchiveSettleInfoBiz.RollBack(id);
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }


        [HttpPost]
        public JsonResult PassReview(string id)
        {
            var success = ArchiveSettleInfoBiz.PassReview(id, AppSession.GetUser());
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }


        [HttpPost]
        public JsonResult SettleOut(string id)
        {
            var success = ArchiveSettleInfoBiz.SettleOut(id);
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }



        //[HttpPost]
        //public JsonResult GetApprovaSettleList(int page, int limit, string orgId)
        //{
        //    var data = ArchiveSettleInfoBiz.GetApprovaSettleList(page, limit, orgId);
        //    return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        //}


        //[HttpPost]
        //public JsonResult GetSettleList(int page, int limit, string orgId)
        //{
        //    var data = ArchiveSettleInfoBiz.Get(page, limit, orgId);
        //    return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        //}


        //[HttpPost]
        //public JsonResult GetArchiveList(int page, int limit, string orgId, string SettleId)
        //{
        //    var data = ArchiveSettleInfoBiz.GetArchiveList(page, limit, orgId, SettleId);
        //    return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        //}


        //[HttpPost]
        //public JsonResult AddSettleDetail(string SettleId, string archiveId)
        //{
        //    var success = ArchiveSettleInfoBiz.AddSettleDetail(SettleId, archiveId);
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}


        //[HttpPost]
        //public JsonResult DelSettleDetail(string SettleId, string archiveId)
        //{
        //    var success = ArchiveSettleInfoBiz.DelSettleDetail(SettleId, archiveId);
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}


        //[HttpPost]
        //public JsonResult Edit(ArchiveSettleInfo data)
        //{
        //    var success = ArchiveSettleInfoBiz.Update(data);
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}

        //[HttpPost]
        //public JsonResult SubmitReview(string id)
        //{
        //    var success = ArchiveSettleInfoBiz.SubmitReview(id);
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}

        //[HttpPost]
        //public JsonResult DelSettleInfo(string id)
        //{
        //    var success = ArchiveSettleInfoBiz.Del(id);
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}


        //[HttpPost]
        //public JsonResult GetSettleDetails(string tId)
        //{
        //    var result = ArchiveSettleInfoBiz.GetDetails(tId);
        //    return Json(new { code = 0, data = result, msg = "" });
        //}


        //[HttpPost]
        //public JsonResult PassReview(string id)
        //{
        //    var success = ArchiveSettleInfoBiz.PassReview(id, AppSession.GetUser());
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}


        //[HttpPost]
        //public JsonResult RollBack(string id)
        //{
        //    var success = ArchiveSettleInfoBiz.RollBack(id);
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}


        //[HttpPost]
        //public ActionResult ExportExcel(string tId)
        //{
        //    //TODO:excel
        //    Stream dataStream = null;
        //    return new FileStreamResult(dataStream, "application/ms-excel") { FileDownloadName = "exportInfo.xlsx" };
        //}


        #endregion
    }
}