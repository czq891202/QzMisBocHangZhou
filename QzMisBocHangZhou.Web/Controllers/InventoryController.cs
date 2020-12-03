using QzMisBocHangZhou.Biz;
using QzMisBocHangZhou.Model;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class InventoryController : Controller
    {
        public ActionResult InventoryListView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");
            return View(AppSession.GetUser());
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public ActionResult ShowView(string tId)
        {
            var result = InventoryInfoBiz.Get(tId);
            return View(result);
        }

        public ActionResult ExportList(string orgId)
        {
            if (string.IsNullOrEmpty(orgId))
                orgId = AppSession.GetUser().OrgId;
            var txt = InventoryInfoBiz.Export(orgId, AppSession.GetUser());

            return File(txt, "application/vnd.ms-txt", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
        }

        public ActionResult ImportView(string tId)
        {
            return View(new InventoryInfo() {Id = tId });
        }

        [HttpPost]
        public JsonResult GetInventoryArchiveList(int page, int limit, string orgId)
        {
            if (string.IsNullOrEmpty(orgId))
                orgId = AppSession.GetUser().OrgId;
            var data = InventoryInfoBiz.GetInventoryArchiveList(page, limit, orgId);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }

        [HttpPost]
        public JsonResult GetInventoryList(int page, int limit)
        {
            var data = InventoryInfoBiz.Get(page, limit);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }

        public ActionResult ImportData(string tId)
        {
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = files[0];

            var path = Path.Combine(Server.MapPath("../InventoryTmp"), $"{Guid.NewGuid().ToString()}.txt");
            file.SaveAs(path);

            var success = InventoryInfoBiz.Import(tId, path);

            System.IO.File.Delete(path);
            return Json(new { code = 0, data = success, msg = "" });
        }

        [HttpPost]
        public JsonResult GetInventoryDetails(int page, int limit, string tId)
        {
            var data = InventoryInfoBiz.GetDetails(page, limit, tId);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }
    }
}