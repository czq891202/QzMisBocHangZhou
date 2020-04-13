using QzMisBocHangZhou.Biz;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class BorrowController : Controller
    {
        #region 【视图控制器】
        /// <summary>
        /// 借阅清单
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
        #endregion

        #region api接口
        [HttpPost]
        public JsonResult GetPreBorrow(int page, int limit, string orgId, string keywords)
        {
            var data = ArchiveBorrowInfoBiz.GetPreBorrow(page, limit, orgId, keywords);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }

        [HttpPost]
        public JsonResult GetPreReview(int page, int limit, string orgId, string keywords)
        {
            var data = ArchiveBorrowInfoBiz.GetPreReview(page, limit, orgId, keywords);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }

        [HttpPost]
        public JsonResult SubmitReview(string tId, string usedBy, DateTime? borrowDate)
        {
            var success = ArchiveBorrowInfoBiz.SubmitReview(tId, usedBy, borrowDate, AppSession.GetUser());
            return Json(new { code = 0, data = success, msg = "" });
        }

        [HttpPost]
        public JsonResult RollBack(string id)
        {
            var success = ArchiveBorrowInfoBiz.RollBack(id);
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }

        [HttpPost]
        public JsonResult PassReview(string id)
        {
            var success = ArchiveBorrowInfoBiz.PassReview(id, AppSession.GetUser());
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }

        public ActionResult ExportBorrowExcel()
        {
            var excel = ExportExcel.ExportBorrow(Server.MapPath("../ExcelTemplate/Borrow.xlsx"), AppSession.GetUser());

            return File(excel, "application/ms-excel", $"零贷档案借阅表 - {DateTime.Now.ToString("yyyyMMdd")}.xlsx");

            //return new FileStreamResult(dataStream, "application/ms-excel") { FileDownloadName = "exportInfo.xlsx" };
        }
        #endregion



        ///// <summary>
        ///// 借阅编辑
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult EditView(string mode, string tId)
        //{
        //    if (string.IsNullOrWhiteSpace(mode) || mode.Equals("add", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(tId))
        //    {
        //        var user = AppSession.GetUser();
        //        var data = ArchiveBorrowInfoBiz.CreatDefault(user.OrgId, user.RealName);
        //        tId = data.Id;
        //    }

        //    var result = ArchiveBorrowInfoBiz.Get(tId);
        //    return View(new EditViewModel<ArchiveBorrowInfo>() { Data = result, User = AppSession.GetUser() });
        //}






        ///// <summary>
        ///// 查看
        ///// </summary>
        ///// <param name="tId"></param>
        ///// <returns></returns>
        //public ActionResult ShowView(string tId)
        //{
        //    var result = ArchiveBorrowInfoBiz.Get(tId);
        //    return View(new EditViewModel<ArchiveBorrowInfo>() { Data = result, User = AppSession.GetUser() });
        //}





        ///// <summary>
        ///// 借阅审批
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult ApprovalView(string tId)
        //{
        //    var result = ArchiveBorrowInfoBiz.Get(tId);
        //    return View(new EditViewModel<ArchiveBorrowInfo>() { Data = result, User = AppSession.GetUser() });
        //}


        //public ActionResult ReturnView(string id)
        //{
        //    var result = ArchiveBorrowInfoBiz.GetDetail(id);
        //    return View(new EditViewModel<ArchiveBorrowDetails>() { Data = result, User = AppSession.GetUser() });
        //}

        //public ActionResult ExportList(string tId)
        //{
        //    var txt = ArchiveBorrowInfoBiz.Export(tId);

        //    return File(txt, "application/vnd.ms-txt", $"{DateTime.Now.ToString("yyyyMMdd")}.txt");
        //}

        //public ActionResult ImportView(string tId)
        //{
        //    var result = ArchiveBorrowInfoBiz.Get(tId);
        //    return View(result);
        //}

        //#endregion



        //#region api接口
        //[HttpPost]
        //public JsonResult GetBorrowList(int page, int limit, string orgId)
        //{
        //    var data = ArchiveBorrowInfoBiz.Get(page, limit, orgId);
        //    return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        //}

        //[HttpPost]
        //public JsonResult SubmitReview(string id)
        //{
        //    var success = ArchiveBorrowInfoBiz.SubmitReview(id);
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}

        //[HttpPost]
        //public JsonResult DelBorrowInfo(string id)
        //{
        //    var success = ArchiveBorrowInfoBiz.Del(id);
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}

        //[HttpPost]
        //public JsonResult GetBorrowDetails(string tId)
        //{
        //    var result = ArchiveBorrowInfoBiz.GetDetails(tId);
        //    return Json(new { code = 0, data = result, msg = "" });
        //}


        //[HttpPost]
        //public JsonResult GetArchiveList(int page, int limit, string orgId, string borrowId)
        //{
        //    var data = ArchiveBorrowInfoBiz.GetArchiveList(page, limit, orgId, borrowId);
        //    return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        //}



        //[HttpPost]
        //public JsonResult EditInfo(ArchiveBorrowInfo info)
        //{
        //    var success = ArchiveBorrowInfoBiz.UpdateInfo(info, AppSession.GetUser());
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}

        //[HttpPost]
        //public JsonResult DelDetail(string id)
        //{
        //    var success = ArchiveBorrowInfoBiz.DelDetails(id);
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}

        //[HttpPost]
        //public JsonResult GetApprovaBorrowList(int page, int limit, string orgId)
        //{
        //    var data = ArchiveBorrowInfoBiz.GetApprovaBorrowList(page, limit, orgId);
        //    return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        //}



        //[HttpPost]
        //public JsonResult PassReview(string id)
        //{
        //    var success = ArchiveBorrowInfoBiz.PassReview(id, AppSession.GetUser());
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}



        //[HttpPost]
        //public JsonResult ReturnArchive(ArchiveBorrowDetails details)
        //{
        //    var success = ArchiveBorrowInfoBiz.ReturnArchive(details);
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}





        //#endregion

        //[HttpPost]
        //public ActionResult VerifyData(string tId)
        //{

        //    HttpFileCollectionBase files = Request.Files;
        //    HttpPostedFileBase file = files[0];

        //    var path = Path.Combine(Server.MapPath("../InventoryTmp"), $"{Guid.NewGuid().ToString()}.txt");
        //    file.SaveAs(path);

        //    var success = ArchiveBorrowInfoBiz.VerifyData(tId, path);

        //    return Json(new { code = 0, data = success, msg = "" });

        //}
    }
}