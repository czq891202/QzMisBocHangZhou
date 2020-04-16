using QzMisBocHangZhou.Biz;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class PrintLabelController : Controller
    {
        // GET: PrintLabel
        public ActionResult PrintLabelListView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");
            return View(AppSession.GetUser());
        }


        [HttpPost]
        public ActionResult GetInfoByYear(string orgId, int year)
        {
            var result = PrintLabelInfoBiz.GetInfo(orgId, year);
            return Json(new { code = 0, data = result, msg = "" });
        }

        [HttpPost]
        public ActionResult GetInfo(string orgId)
        {
            var result = PrintLabelInfoBiz.GetInfo(orgId);
            return Json(new { code = 0, data = result, msg = "" });
        }

        [HttpPost]
        public ActionResult Edit(PrintLabelInfo printLabelInfo)
        {
            var result = PrintLabelInfoBiz.Edit(printLabelInfo);
            return Json(new { code = 0, data = result, msg = "" });
        }

        [HttpPost]
        public ActionResult GetPrintInfo(string labelNo)
        {
            if(string.IsNullOrWhiteSpace(labelNo)) return Json(new { code = 0, data = false, msg = "标签号错误!" });

            var result = PrintLabelInfoBiz.GetPrintInfo(labelNo);

            return Json(new { code = 0, data = result, msg = "" });
        }
    }
}