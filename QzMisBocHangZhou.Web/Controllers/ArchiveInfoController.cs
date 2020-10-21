using QzMisBocHangZhou.Biz;
using QzMisBocHangZhou.Model;
using System;
using System.Data;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class ArchiveInfoController : Controller
    {
        #region 【视图】
        // GET: ArchiveInfo
        public ActionResult ArchiveInfoView()
        {
            return View(new EditViewModel<ArchiveInfo>());
        }

        /// <summary>
        /// 档案列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ArchiveInfoListView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");

            return View(AppSession.GetUser());
        }

        /// <summary>
        /// 档案编辑
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="id"></param>
        /// <param name="tpId"></param>
        /// <returns></returns>
        public ActionResult ArchiveInfoEditView(string mode, string id, string tpId)
        {
            if (string.IsNullOrWhiteSpace(id) || mode.Equals("add", StringComparison.OrdinalIgnoreCase))
            {
                return View(new EditViewModel<ArchiveInfo>() { Data = new ArchiveInfo(), User = AppSession.GetUser() });
            }
            else
            {
                var result = ArchiveInfoBiz.Get(id);
                return View(new EditViewModel<ArchiveInfo>() { Data = result, User = AppSession.GetUser() });
            }
        }
        #endregion

        #region 【档案】
        [HttpPost]
        public JsonResult GetArchiveInfoList(int page, int limit, string keywords, string orgId, int status)
        {
            var data = ArchiveInfoBiz.PagingQuery(page, limit, keywords, orgId, status);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }

        public ActionResult ExportArchiveOutExcel(string orgId, string keywords, int status)
        {
            var infos = ArchiveInfoBiz.GetPreOut(orgId, keywords, status);

            DataTable dataTable = new DataTable();
            DataColumn column = dataTable.Columns.Add("序号", Type.GetType("System.Int32"));
            column.AutoIncrement = true;//自动增长
            column.AutoIncrementSeed = 1;//起始为1
            column.AutoIncrementStep = 1;//步长为1
            column.AllowDBNull = false;
            column = dataTable.Columns.Add("机构名称", Type.GetType("System.String"));
            column = dataTable.Columns.Add("机构代码", Type.GetType("System.String"));
            column = dataTable.Columns.Add("额度号", Type.GetType("System.String"));
            column = dataTable.Columns.Add("贷款账号", Type.GetType("System.String"));
            column = dataTable.Columns.Add("押品类型", Type.GetType("System.String"));
            column = dataTable.Columns.Add("借款人", Type.GetType("System.String"));
            column = dataTable.Columns.Add("贷款金额", Type.GetType("System.String"));
            column = dataTable.Columns.Add("标签号", Type.GetType("System.String"));
            column = dataTable.Columns.Add("客户号", Type.GetType("System.String"));
            column = dataTable.Columns.Add("状态", Type.GetType("System.String"));

            foreach (ArchiveInfo info in infos)
            {
                DataRow dr = dataTable.NewRow();
                dr["机构名称"] = info.OrgName;
                dr["机构代码"] = info.OrgCode;
                dr["额度号"] = info.QuotaNo;
                dr["贷款账号"] = info.LoanAccount;
                //dr["押品类型"] = info.GuaranteeType;
                switch (info.GuaranteeType)
                {
                    case "1":
                        dr["押品类型"] = "已办妥正式抵押登记(他项权证)";
                        break;
                    case "2":
                        dr["押品类型"] = "已办妥预抵押登记";
                        break;
                    case "3":
                        dr["押品类型"] = "已办妥备案登记";
                        break;
                    case "4":
                        dr["押品类型"] = "未办妥抵押（抵押）登记";
                        break;
                    case "7":
                        dr["押品类型"] = "不需抵质押我行";
                        break;
                    default:
                        dr["押品类型"] = "";
                        break;
                }

                dr["借款人"] = info.Borrower;
                dr["贷款金额"] = info.LoanAmount;
                dr["标签号"] = info.LabelCode;
                dr["客户号"] = info.CustomerNo;
                switch (info.Status)
                {
                    case ArchiveStatusType.草稿:
                        dr["状态"] = "未移交";
                        break;
                    case ArchiveStatusType.借阅出库:
                        dr["状态"] = "借阅";
                        break;
                    case ArchiveStatusType.已结清:
                        dr["状态"] = "结清";
                        break;
                    default:
                        dr["状态"] = "在库";
                        break;
                }                
                dataTable.Rows.Add(dr);
            }

            var excel = ExportExcel.DataTableToExcel(dataTable);

            return File(excel, "application/ms-excel", $"档案清单 - {DateTime.Now.ToString("yyyyMMdd")}.xlsx");
        }

        [HttpPost]
        public JsonResult Edit(ArchiveInfo data)
        {
            var success = false;
            if (string.IsNullOrWhiteSpace(data.Id))
            {
                success = ArchiveInfoBiz.Add(data);
            }
            else
            {
                success = ArchiveInfoBiz.Update(data);
            }

            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }
        #endregion                             
    }
}