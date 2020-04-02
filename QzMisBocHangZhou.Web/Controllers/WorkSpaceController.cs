using QzMisBocHangZhou.Biz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class WorkSpaceController : Controller
    {
        // GET: WorkSpace
        public ActionResult WorkSpaceView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");
            
            return View(WorkSpaceBiz.GetWorkSpaceInfo(AppSession.GetUser()));
        }
    }
}