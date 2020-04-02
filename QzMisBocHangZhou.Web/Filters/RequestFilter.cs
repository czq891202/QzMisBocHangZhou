using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace QzMisBocHangZhou.Web.Filters
{
    /// <summary>
    /// API返回值处理
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class RequestFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 重写调用前请求(同步)
        /// </summary>
        /// <param name="actionContext">请求上下文</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //不预处理
        }
    }
}