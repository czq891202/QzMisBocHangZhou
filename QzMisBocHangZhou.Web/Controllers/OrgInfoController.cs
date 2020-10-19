using QzMisBocHangZhou.Biz;
using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class OrgInfoController : Controller
    {
        // GET: OrgInfo
        public ActionResult OrgInfoListView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");

            return View(AppSession.GetUser());
        }

        /// <summary>
        /// 编辑页控制器
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="id"></param>
        /// <param name="pId"></param>
        /// <returns></returns>
        public ActionResult OrgInfoEditView(string mode, string id, string pId)
        {
            if (!string.IsNullOrWhiteSpace(mode) && mode.Equals("edit", StringComparison.OrdinalIgnoreCase))
            {
                return View(OrgInfoBiz.Get(id));
            }
            else
            {
                var result = new OrgInfo();
                if (!string.IsNullOrWhiteSpace(pId)) result.ParentId = pId;
                return View(result);
            }
        }

        [HttpGet]
        public JsonResult GetOrgInfoList()
        {
            var result = OrgInfoBiz.GetAll();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取组织树
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetOrgTree(string orgid)
        {
            var result = OrgInfoBiz.GetOrgTree(orgid);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOrgInfoListByUser()
        {
            var orgid = AppSession.GetUser().OrgId;
            var result = OrgInfoBiz.GetOrgTree(orgid);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Edit(OrgInfo data)
        {
            var success = false;
            if (string.IsNullOrWhiteSpace(data.Id))
            {
                success = OrgInfoBiz.Add(data);
            }
            else
            {
                success = OrgInfoBiz.Update(data);
            }

            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }

        public static List<OrgInfo>  GetAllParent(string orgid)
        {
            return OrgInfoBiz.GetAllParent(orgid);
        }
    }
}