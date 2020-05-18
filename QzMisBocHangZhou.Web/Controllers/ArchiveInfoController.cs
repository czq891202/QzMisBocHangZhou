using QzMisBocHangZhou.Biz;
using QzMisBocHangZhou.Model;
using System;
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