using QzMisBocHangZhou.Biz;
using System.Web.Mvc;
//using QzMisBocHangZhou.Biz;

namespace QzMisBocHangZhou.Web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult LoginView()
        {
            AppSession.Clear();
            return View();
        }

        [HttpPost]
        public JsonResult Login(string userName, string password)
        {
            var result = UserInfoBiz.Login(userName, password);

            if (result)
            {
                CacheInfo(userName);
                if(AppSession.GetUser().LastLandingTime.GetType() != typeof(System.DBNull))
                {
                    UserInfoBiz.SetLastLandingTime(AppSession.GetUser());
                }
            }
            return Json(new { data = result, msg = result ? "" : "error!" });
        }
        /// <summary>
        /// 缓存登陆用户信息 
        /// </summary>
        /// <param name="userName"></param>
        private void CacheInfo(string userName)
        {
            var userInfo = UserInfoBiz.GetDetailsByName(userName);
            AppSession.AddUser(userInfo);

            var role = RoleInfoBiz.Get(userInfo.RoleId);
            AppSession.AddRole(role);

            var roleValue = RoleInfoBiz.GetRoleValue(userInfo.RoleId);
            AppSession.AddRoleValue(roleValue);
        }
    }
}