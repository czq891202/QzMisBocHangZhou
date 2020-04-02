using QzMisBocHangZhou.Biz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class TPArchiveInfoController : Controller
    {
        // GET: TPArchiveInfo
        public ActionResult TPArchiveInfoListView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");

            
            return View(AppSession.GetUser());
        }

        [HttpPost]
        public JsonResult GetTPArchiveInfoList(int page, int limit, string keywords, string orgId)
        {
            //var data = TPArchiveInfoBiz.PagingQuery(page, limit, keywords, orgId);
            //return Json(new { code = 0, count = data.Count, data = data.Result, msg = "" });
        }
    }
}