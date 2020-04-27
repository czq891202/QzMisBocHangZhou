using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QzMisBocHangZhou.Biz;
using System.IO;
using System.Data;

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

        /// <summary>
        /// 出库清单导出
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportSettleOutExcel()
        {
            var infos = ArchiveSettleInfoBiz.GetPreOut();

            DataTable dataTable = new DataTable();
            DataColumn column = dataTable.Columns.Add("序号", Type.GetType("System.Int32"));
            column.AutoIncrement = true;//自动增长
            column.AutoIncrementSeed = 1;//起始为1
            column.AutoIncrementStep = 1;//步长为1
            column.AllowDBNull = false;
            column = dataTable.Columns.Add("一级支行", Type.GetType("System.String"));
            column = dataTable.Columns.Add("二级支行", Type.GetType("System.String"));
            column = dataTable.Columns.Add("借款人", Type.GetType("System.String"));
            column = dataTable.Columns.Add("存放地址", Type.GetType("System.String"));
            column = dataTable.Columns.Add("产品大类", Type.GetType("System.String"));

            foreach (ArchiveSettleInfo info in infos)
            {
                DataRow dr = dataTable.NewRow();
                var orginfo = OrgInfoBiz.GetAllParent(info.OrgId);
                dr["一级支行"] = orginfo.Count >= 1 ? orginfo[orginfo.Count - 1].Name : "";
                dr["二级支行"] = orginfo.Count > 1 ? orginfo[orginfo.Count - 2].Name : "";
                dr["借款人"] = info.LoanBorrower;
                dr["存放地址"] = info.StorageLocation;
                dr["产品大类"] = info.ProductCode;
                dataTable.Rows.Add(dr);
            }

            var excel = ExportExcel.DataTableToExcel(dataTable);

            return File(excel, "application/ms-excel", $"结清出库清单 - {DateTime.Now.ToString("yyyyMMdd")}.xlsx");
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
        #endregion
    }
}