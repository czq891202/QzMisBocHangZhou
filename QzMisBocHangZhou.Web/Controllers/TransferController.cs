using QzMisBocHangZhou.Biz;
using QzMisBocHangZhou.Model;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class TransferController : Controller
    {
        #region 【移交视图控制器】
        /// <summary>
        /// 移交清单
        /// </summary>
        /// <returns></returns>
        public ActionResult ListView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");
            return View(AppSession.GetUser());
        }


        public ActionResult TransferEditView(string id)
        {
            var result = ArchiveInfoBiz.Get(id);
            return View(new EditViewModel<ArchiveInfo>() { Data = result, User = AppSession.GetUser() });
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

        #region 接口
        [HttpPost]
        public JsonResult GetPreTransfer(int page, int limit, string orgId, string keywords)
        {
            var data = ArchiveTransferInfoBiz.GetPreTransfer(page, limit, orgId, keywords);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }

        [HttpPost]
        public JsonResult GetPreReview(int page, int limit, string orgId, string keywords)
        {
            var data = ArchiveTransferInfoBiz.GetPreReview(page, limit, orgId, keywords);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }

        [HttpPost]
        public JsonResult SubmitReview(ArchiveInfo data)
        {
            var success = ArchiveTransferInfoBiz.SubmitReview(data, AppSession.GetUser());
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }

        [HttpPost]
        public JsonResult RollBack(string id)
        {
            var success = ArchiveTransferInfoBiz.RollBack(id);
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }

        [HttpPost]
        public JsonResult PassReview(string id)
        {
            var success = ArchiveTransferInfoBiz.PassReview(id, AppSession.GetUser());
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }

        public ActionResult ExportTransferExcel()
        {
            var excel = ExportExcel.ExportTransfer(Server.MapPath("../ExcelTemplate/Transfer.xlsx"), AppSession.GetUser());

            return File(excel, "application/ms-excel", $"零贷档案交接单 - {DateTime.Now.ToString("yyyyMMdd")}.xlsx");
        }
        /// <summary>
        /// 移交待审核电子数据核对
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public ActionResult ExportList(string orgId)
        {
            var txt = ArchiveTransferInfoBiz.Export(orgId, AppSession.GetUser());

            return File(txt, "application/vnd.ms-txt", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
        }
        #endregion

        #region 【作废】

        //[HttpPost]
        //public JsonResult GetApprovaTransferList(int page, int limit, string orgId)
        //{
        //    var data = ArchiveTransferInfoBiz.GetApprovaTransferList(page, limit, orgId);
        //    return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        //}





        //[HttpPost]
        //public JsonResult GetArchiveList(int page, int limit, string orgId, string transferId)
        //{
        //    var data = ArchiveTransferInfoBiz.GetArchiveList(page, limit, orgId, transferId);
        //    return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        //}


        //[HttpPost]
        //public JsonResult AddTransferDetail(string transferId, string archiveId)
        //{
        //    var success = ArchiveTransferInfoBiz.AddTransferDetail(transferId, archiveId);
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}


        //[HttpPost]
        //public JsonResult DelTransferDetail(string transferId, string archiveId)
        //{
        //    var success = ArchiveTransferInfoBiz.DelTransferDetail(transferId, archiveId);
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}


        //[HttpPost]
        //public JsonResult Edit(ArchiveTransferInfo data)
        //{
        //    var success = ArchiveTransferInfoBiz.Update(data);
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}



        //[HttpPost]
        //public JsonResult DelTransferInfo(string id)
        //{
        //    var success = ArchiveTransferInfoBiz.Del(id);
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}


        //[HttpPost]
        //public JsonResult GetTransferDetails(string tId)
        //{
        //    var result = ArchiveTransferInfoBiz.GetDetails(tId);
        //    return Json(new { code = 0, data = result, msg = "" });
        //}


        //[HttpPost]
        //public ActionResult VerifyData(string tId)
        //{

        //    HttpFileCollectionBase files = Request.Files;
        //    HttpPostedFileBase file = files[0];

        //    var path = Path.Combine(Server.MapPath("../InventoryTmp"), $"{Guid.NewGuid().ToString()}.txt");
        //    file.SaveAs(path);

        //    var success = ArchiveTransferInfoBiz.VerifyData(tId, path, AppSession.GetUser());

        //    return Json(new { code = 0, data = success, msg = "" });

        //}

        #endregion


        #region 【作废】
        ///// <summary>
        ///// 移交编辑
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult EditView(string mode, string tId)
        //{
        //    //if (string.IsNullOrWhiteSpace(mode) || mode.Equals("add", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(tId))
        //    //{
        //    //    var user = AppSession.GetUser();
        //    //    var data = ArchiveTransferInfoBiz.CreatDefault(user.OrgId, user.RealName);
        //    //    tId = data.Id;
        //    //    //return View(new EditViewModel<ArchiveTransferInfo>() { Data = result, User = AppSession.GetUser() });
        //    //}

        //    //var result = ArchiveTransferInfoBiz.Get(tId);
        //    //return View(new EditViewModel<ArchiveTransferInfo>() { Data = result, User = AppSession.GetUser() });

        //    return View();

        //}

        ///// <summary>
        ///// 查看
        ///// </summary>
        ///// <param name="tId"></param>
        ///// <returns></returns>
        //public ActionResult ShowView(string tId)
        //{
        //    //var result = ArchiveTransferInfoBiz.Get(tId);
        //    //return View(result);

        //    return View();
        //}


        ///// <summary>
        ///// 移交审批
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult ApprovalView(string tId)
        //{
        //    //var result = ArchiveTransferInfoBiz.Get(tId);
        //    //return View(result);

        //    return View();
        //}


        //public ActionResult ExportList(string tId)
        //{
        //    //var txt = ArchiveTransferInfoBiz.Export(tId);

        //    //return File(txt, "application/vnd.ms-txt", $"{DateTime.Now.ToString("yyyyMMdd")}.txt");

        //    return View();
        //}

        //public ActionResult ImportView(string tId)
        //{
        //    //var result = ArchiveTransferInfoBiz.Get(tId);
        //    //return View(result);
        //    return View();
        //}

        #endregion
    }
}