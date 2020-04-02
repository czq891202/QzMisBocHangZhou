using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult MainView()
        {            
            if(AppSession.IsExits())
            {
                return View(AppSession.GetUser());
            }
            else
            {
                return Redirect("/Login/LoginView");
            }
        }
    }
}