using QzMisBocHangZhou.Biz;
using QzMisBocHangZhou.Model;
using System;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Controllers
{
    /// <summary>
    /// 归还清单
    /// </summary>
    /// <returns></returns>
    public class GiveBackController : Controller
    {
        // GET: GiveBack
        public ActionResult ListView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");
            return View(AppSession.GetUser());
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

        /// <summary>
        /// 归还变更编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeInEditView(string borrowId)
        {
            ViewData["BorrowId"] = borrowId;

            var arcId = ArchiveGiveBackInfoBiz.Get(borrowId).ArchiveId;
            var result = ArchiveInfoBiz.Get(arcId);
            return View(new EditViewModel<ArchiveInfo>() { Data = result, User = AppSession.GetUser() });
        }

        /// <summary>
        /// 归还审批弹框
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public ActionResult AppendArchiveView(string bId)
        {
            return View(new EditViewModel<string>() { Data = bId, User = AppSession.GetUser() });
        }
        
        #region api
        /// <summary>
        /// 归还提交审批
        /// </summary>
        /// <param name="bId">借阅Id</param>
        /// <param name="givebackDate">归还时间</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SubmitGiveBack(string bId, DateTime? givebackDate)
        {
            var success = ArchiveGiveBackInfoBiz.SubmitGiveBack(bId, givebackDate, AppSession.GetUser());
            return Json(new { code = 0, data = success, msg = "" });
        }
        /// <summary>
        /// 获取待归还审批列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="orgId">组织Id</param>
        /// <param name="keywords">搜索内容</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetGiveBackReview(int page, int limit, string orgId, string keyWords)
        {
            var data = ArchiveGiveBackInfoBiz.GetGiveBackReview(page, limit, orgId, keyWords);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }
        /// <summary>
        /// 获取待归还列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="orgId"></param>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPreGiveBack(int page, int limit, string orgId, string keyWords)
        {
            var data = ArchiveGiveBackInfoBiz.GetPreGiveBack(page, limit, orgId, keyWords);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }
        /// <summary>
        /// 归还审批提交撤回
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RollBack(string id)
        {
            var success = ArchiveGiveBackInfoBiz.GiveBackRollBack(id);
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }

        [HttpPost]
        public JsonResult ChangeIn(ArchiveInfo data, string borrowId)
        {
            var success = ArchiveBorrowInfoBiz.ChangeIn(data, borrowId);
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }

        [HttpPost]
        public JsonResult Returned(string id)
        {
            var success = ArchiveGiveBackInfoBiz.Returned(id);
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }
        
        public ActionResult ExportList(string orgId)
        {
            var txt = ArchiveGiveBackInfoBiz.Export(orgId, AppSession.GetUser());

            return File(txt, "application/vnd.ms-txt", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
        }
        #endregion
    }
}