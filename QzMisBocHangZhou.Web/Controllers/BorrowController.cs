using QzMisBocHangZhou.Biz;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Data;
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
        /// 借阅清单视图
        /// </summary>
        /// <returns></returns>
        public ActionResult ListView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");
            return View(AppSession.GetUser());
        }
        /// <summary>
        /// 借阅审核视图
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public ActionResult AppendArchiveView(string tId)
        {
            return View(new EditViewModel<string>() { Data = tId, User = AppSession.GetUser() });
        }
        #endregion

        #region api接口
        /// <summary>
        /// 审批列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ApprovalListView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");
            return View(AppSession.GetUser());
        }
        /// <summary>
        /// 可借阅清单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="orgId"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPreBorrow(int page, int limit, string orgId, string keywords)
        {
            var data = ArchiveBorrowInfoBiz.GetPreBorrow(page, limit, orgId, keywords);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }
        /// <summary>
        /// 借阅待审核清单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="orgId"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPreReview(int page, int limit, string orgId, string keywords)
        {
            var data = ArchiveBorrowInfoBiz.GetPreReview(page, limit, orgId, keywords);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }
        /// <summary>
        /// 借阅待出库清单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="orgId"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPreOut(int page, int limit, string orgId, string keywords)
        {
            var data = ArchiveBorrowInfoBiz.GetPreOut(page, limit, orgId, keywords);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }
        /// <summary>
        /// 提交借阅
        /// </summary>
        /// <param name="tId"></param>
        /// <param name="usedBy"></param>
        /// <param name="borrowDate"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SubmitReview(string tId, string usedBy, DateTime? borrowDate)
        {
            var success = ArchiveBorrowInfoBiz.SubmitReview(tId, usedBy, borrowDate, AppSession.GetUser());
            return Json(new { code = 0, data = success, msg = "" });
        }
        /// <summary>
        /// 借阅撤回/驳回
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RollBack(string id)
        {
            var success = ArchiveBorrowInfoBiz.RollBack(id);
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }
        /// <summary>
        /// 借阅通过
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PassReview(string id)
        {
            var success = ArchiveBorrowInfoBiz.PassReview(id, AppSession.GetUser());
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }
        /// <summary>
        /// 借阅出库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult BorrowOut(string id)
        {
            var success = ArchiveBorrowInfoBiz.BorrowOut(id, AppSession.GetUser());
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }
        /// <summary>
        /// 借阅待审核清单导出
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportBorrowExcel()
        {
            var excel = ExportExcel.ExportBorrow(Server.MapPath("../ExcelTemplate/Borrow.xlsx"), AppSession.GetUser());

            return File(excel, "application/ms-excel", $"零贷档案待审核借阅表 - {DateTime.Now.ToString("yyyyMMdd")}.xlsx");
        }
        /// <summary>
        /// 借阅清单导出
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportBorrowListExcel(string orgId, string keywords)
        {
            var excel = ExportExcel.ExportBorrowListExcel(Server.MapPath("../ExcelTemplate/Borrow.xlsx"), AppSession.GetUser(), orgId, keywords);

            return File(excel, "application/ms-excel", $"零贷档案借阅表 - {DateTime.Now.ToString("yyyyMMdd")}.xlsx");
        }
        /// <summary>
        /// 借阅出库清单导出
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportBorrowOutExcel()
        {
            var infos = ArchiveBorrowInfoBiz.GetPreOut();

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

            foreach(ArchiveBorrowInfo info in infos)
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

            return File(excel, "application/ms-excel", $"借阅出库清单 - {DateTime.Now.ToString("yyyyMMdd")}.xlsx");
        }
        #endregion
    }
}