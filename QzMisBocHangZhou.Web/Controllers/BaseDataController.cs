using QzMisBocHangZhou.Biz;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class BaseDataController : Controller
    {
        // GET: BaseData
        public ActionResult ArchiveProcTimeView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");
            return View();
        }

        [HttpPost]
        public ActionResult GetArchiveProcTimeList()
        {
            var result = BaseDataBiz.GetArchiveProcTimeList();
            return Json(new { code = 0, data = result, msg = "" });
        }

        public ActionResult EditArchiveProcTime(ArchiveProcTime data)
        {
            var success = BaseDataBiz.UpDateArchiveProcTime(data);
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }
    }
}