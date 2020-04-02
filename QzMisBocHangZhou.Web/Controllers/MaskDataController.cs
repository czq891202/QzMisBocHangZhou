using QzMisBocHangZhou.Biz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class MaskDataController : Controller
    {
        // GET: MaskData
        public ActionResult MaskDataListView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");
            return View();
        }


        public ActionResult ImportView()
        {
            return View();
        }

        public ActionResult ImportData()
        {

            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = files[0];

            var path = Path.Combine(Server.MapPath("../InventoryTmp"), $"{Guid.NewGuid().ToString()}.txt");
            file.SaveAs(path);
            var data = System.IO.File.ReadAllLines(path);
            var success = MaskDataBiz.Add(data.ToList());

            try
            {
                System.IO.File.Delete(path);
            }
            catch { }

            return Json(new { code = 0, data = success, msg = "" });

        }


        [HttpPost]
        public JsonResult GetMaskDataList(int page, int limit)
        {
            var data = MaskDataBiz.Get(page, limit);
            return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }


        [HttpPost]
        public JsonResult DelMaskData(string id)
        {
            var success = MaskDataBiz.Del(id);
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }
    }
}