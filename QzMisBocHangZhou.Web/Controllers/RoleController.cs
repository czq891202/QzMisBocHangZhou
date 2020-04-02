using QzMisBocHangZhou.Biz;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class RoleController : Controller
    {
        // GET: Role
        public ActionResult RoleListView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");

            return View();
        }

        public ActionResult RoleEditView(string mode, string id)
        {
            if (!string.IsNullOrWhiteSpace(mode) && mode.Equals("edit", StringComparison.OrdinalIgnoreCase))
            {
                return View(RoleInfoBiz.Get(id));
            }
            else
            {
                return View(new Role());
            }
        }


        [HttpPost]
        public JsonResult GetRoleInfoList()
        {
            var result = RoleInfoBiz.Get();
            return Json(new { code = 0, data = result, msg = "" });
        }


        [HttpPost]
        public JsonResult GetRoleValueInfoList(string id)
        {
            var roleValues = RoleInfoBiz.GetRoleValue(id);
            var result = roleValues.Select(p => p.NavigationId).ToArray();
            return Json(new ResultModel<string[]>(result));
        }


        [HttpPost]
        public JsonResult Edit(RoleViewModel data)
        {
            var success = false;
            if (string.IsNullOrWhiteSpace(data.RoleInfo.Id))
            {
                success = RoleInfoBiz.Add(data.RoleInfo, data.NavIds);
            }
            else
            {
                success = RoleInfoBiz.Update(data.RoleInfo, data.NavIds);
            }

            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }


        [HttpPost]
        public JsonResult Del(string id)
        {
            var success = RoleInfoBiz.Del(id);
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }
    }
}