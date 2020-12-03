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
            if (string.IsNullOrEmpty(orgId))
                orgId = AppSession.GetUser().OrgId;
            var data = ArchiveTransferInfoBiz.GetPreTransfer(page, limit, orgId, keywords);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }

        [HttpPost]
        public JsonResult GetPreReview(int page, int limit, string orgId, string keywords)
        {
            if (string.IsNullOrEmpty(orgId))
                orgId = AppSession.GetUser().OrgId;
            var data = ArchiveTransferInfoBiz.GetPreReview(page, limit, orgId, keywords);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }

        [HttpPost]
        public JsonResult SubmitReview(ArchiveInfo data)
        {
            if (ArchiveTransferInfoBiz.CheckLabelCode(data.LabelCode, data.CustomerNo))
            {
                return Json(new ResultModel<string>() { msg = "该电子标签已经使用!" });
            }
            else
            {
                var success = ArchiveTransferInfoBiz.SubmitReview(data, AppSession.GetUser());
                return Json(new ResultModel<string>() { msg = success ? "" : "error" });
            }
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

            return File(excel, "application/ms-excel", $"零贷档案待审核交接单 - {DateTime.Now.ToString("yyyyMMdd")}.xlsx");
        }

        public ActionResult ExportTransferListExcel(string orgId, string keywords)
        {
            if (string.IsNullOrEmpty(orgId))
                orgId = AppSession.GetUser().OrgId;
            var excel = ExportExcel.ExportTransferList(Server.MapPath("../ExcelTemplate/Transfer.xlsx"), AppSession.GetUser(), orgId, keywords);

            return File(excel, "application/ms-excel", $"零贷档案交接单 - {DateTime.Now.ToString("yyyyMMdd")}.xlsx");
        }

        /// <summary>
        /// 移交待审核电子数据核对
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public ActionResult ExportList(string orgId)
        {
            if (string.IsNullOrEmpty(orgId))
                orgId = AppSession.GetUser().OrgId;
            var txt = ArchiveTransferInfoBiz.Export(orgId, AppSession.GetUser());

            return File(txt, "application/vnd.ms-txt", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
        }
        #endregion
    }
}