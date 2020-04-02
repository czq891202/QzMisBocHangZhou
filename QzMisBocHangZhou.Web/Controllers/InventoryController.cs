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



        //#region 【视图】
        //public ActionResult ListView()
        //{
        //    if (!AppSession.IsExits()) return Redirect("/Login/LoginView");
        //    return View(AppSession.GetUser());
        //}

        


        //public ActionResult EditView(string mode, string tId)
        //{
        //    if (string.IsNullOrWhiteSpace(mode) || mode.Equals("add", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(tId))
        //    {
        //        var user = AppSession.GetUser();
        //        var data = InventoryInfoBiz.CreatDefault(user.RealName, user.OrgId);
        //        tId = data.Id;
        //        //return View(new EditViewModel<ArchiveTransferInfo>() { Data = result, User = AppSession.GetUser() });
        //    }

        //    var result = InventoryInfoBiz.Get(tId);
        //    return View(new EditViewModel<InventoryInfo>() { Data = result, User = AppSession.GetUser() });

        //}



        //public ActionResult ExportList(string tId)
        //{
        //    var txt = InventoryInfoBiz.Export(tId);

        //    return File(txt, "application/vnd.ms-txt", $"{DateTime.Now.ToString("yyyyMMdd")}.txt");
        //}

        //public ActionResult ImportView(string tId)
        //{
        //    var result = InventoryInfoBiz.Get(tId);
        //    return View(result);
        //}


        //public ActionResult ImportData(string tId)
        //{

        //    HttpFileCollectionBase files = Request.Files;
        //    HttpPostedFileBase file = files[0];

        //    var path = Path.Combine(Server.MapPath("../InventoryTmp"), $"{Guid.NewGuid().ToString()}.txt");
        //    file.SaveAs(path);

        //    var success = InventoryInfoBiz.Import(tId, path);

        //    System.IO.File.Delete(path);
        //    return Json(new { code = 0, data = success, msg = "" });

        //}
        //#endregion


        //#region api接口
        //[HttpPost]
        //public JsonResult GetInventoryList(int page, int limit, string year)
        //{
        //    var data = InventoryInfoBiz.Get(page, limit, year);
        //    return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        //}


        //[HttpPost]
        //public JsonResult DelInventoryInfo(string id)
        //{
        //    var success = InventoryInfoBiz.Del(id);
        //    return Json(new { code = 0, data = success, msg = "" });
        //}


        //[HttpPost]
        //public JsonResult GetArchiveList(int page, int limit, string inventoryId)
        //{
        //    var data = InventoryInfoBiz.GetArchiveList(page, limit, inventoryId);
        //    return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        //}



        //[HttpPost]
        //public JsonResult AddInventoryDetail(string inventoryId, string archiveId)
        //{
        //    var success = InventoryInfoBiz.AddDetail(inventoryId, archiveId);
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}


        //[HttpPost]
        //public JsonResult DelInventoryDetail(string inventoryId, string archiveId)
        //{
        //    var success = InventoryInfoBiz.DelDetail(inventoryId, archiveId);
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}


        //[HttpPost]
        //public JsonResult Edit(InventoryInfo data)
        //{
        //    var success = InventoryInfoBiz.Update(data);
        //    return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        //}





        //#endregion
    }
}