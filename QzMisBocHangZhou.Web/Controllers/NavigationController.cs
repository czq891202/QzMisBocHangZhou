using QzMisBocHangZhou.Biz;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class NavigationController : Controller
    {
        // GET: Navigation
        /// <summary>
        /// 列表页控制器
        /// </summary>
        /// <returns></returns>
        public ActionResult NavigationListView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");

            return View();

        }


        /// <summary>
        /// 编辑页控制器
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="id"></param>
        /// <param name="pId"></param>
        /// <returns></returns>
        public ActionResult NavigationEditView(string mode, string id, string pId)
        {
            if (!string.IsNullOrWhiteSpace(mode) && mode.Equals("edit", StringComparison.OrdinalIgnoreCase))
            {
                return View(NavigationBiz.Get(id));
            }
            else
            {
                var result = new Navigation();
                if (!string.IsNullOrWhiteSpace(pId)) result.ParentId = pId;

                return View(result);
            }
        }


        [HttpGet]
        public JsonResult GetNavigationList()
        {
            var result = NavigationBiz.Get();
            return Json(result, JsonRequestBehavior.AllowGet);
            //return Json(new ResultModel<List<Navigation>>(result), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetNavigationListByRole(string roleId)
        {
            var result = NavigationBiz.GetByRole(roleId);
            return Json(result);
        }


        [HttpPost]
        public JsonResult Edit(Navigation data)
        {
            var success = false;
            if (string.IsNullOrWhiteSpace(data.Id))
            {
                success = NavigationBiz.Add(data);
            }
            else
            {
                success = NavigationBiz.Update(data);
            }

            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }


        [HttpPost]
        public JsonResult Del(string id)
        {
            var success = NavigationBiz.Del(id);
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }
    }
}