using QzMisBocHangZhou.Biz;
using QzMisBocHangZhou.Model;
using System;
using System.Web.Mvc;


namespace QzMisBocHangZhou.Web.Controllers
{
    public class UserInfoController : Controller
    {
        #region 【View】
        public ActionResult UserListView()
        {
            if (!AppSession.IsExits()) return Redirect("/Login/LoginView");
            return View();
        }

        public ActionResult UserEditView(string mode, string id)
        {
            if (!string.IsNullOrWhiteSpace(mode) && mode.Equals("edit", StringComparison.OrdinalIgnoreCase))
            {
                return View(UserInfoBiz.Get(id));
            }
            else
            {
                return View(new UserInfo());
            }
        }

        public ActionResult UserMobileEditView()
        {
            return View(UserInfoBiz.Get(AppSession.GetUser().Id));
        }

        public ActionResult UserPasswordEditView()
        {
            return View(AppSession.GetUser());
        }

        #endregion
        [HttpPost]
        public JsonResult GetUserInfoList()
        {
            var userList = UserInfoBiz.Get();
            return Json(new { code = 0, data = userList, msg = "" });
        }

        [HttpPost]
        public JsonResult SetUserStatus(string userId, int status)
        {
            var success = UserInfoBiz.SetUserStatus(userId, status);
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }

        [HttpPost]
        public JsonResult Edit(UserInfo data)
        {
            var success = false;
            if (string.IsNullOrWhiteSpace(data.Id))
            {
                success = UserInfoBiz.Add(data);
            }
            else
            {
                success = UserInfoBiz.Update(data);
            }
            return Json(new ResultModel<string>() { msg = success ? "" : "error" });
        }

        [HttpPost]
        public JsonResult IsExistsUser(string userName)
        {
            var success = UserInfoBiz.IsExistsUser(userName);
            return Json(new ResultModel<bool>() { data = success });
        }

        [HttpPost]
        public JsonResult ChangeMobile(string id, string mobile)
        {
            var success = UserInfoBiz.ChangeMobile(id, mobile);
            return Json(new ResultModel<bool>() {data = success,  msg = success ? "" : "error" });
        }

        [HttpPost]
        public JsonResult ChangePassword(string id, string password)
        {
            var success = UserInfoBiz.ChangePassword(id, password);
            if (AppSession.GetUser().LastLandingTime == null || AppSession.GetUser().LastLandingTime.GetType() == typeof(System.DBNull))
            {
                UserInfoBiz.SetLastLandingTime(AppSession.GetUser());
            }
            return Json(new ResultModel<bool>() { data = success, msg = success ? "" : "error" });
        }
    }
}