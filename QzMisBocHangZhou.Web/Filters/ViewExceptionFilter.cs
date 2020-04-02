using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QzMisBocHangZhou.Web.Filters
{
    public class ViewExceptionFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            LoggerExceptionInfo(filterContext);
            base.OnException(filterContext);
        }


        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="actionExecutedContext">执行的上下文的操作</param>
        private static void LoggerExceptionInfo(ExceptionContext filterContext)
        {
            //var actionName = FilterUtils.GetActionFullName();
            //var args = FilterUtils.GetRequestArgsJson(filterContext);

            //var info = $"调用接口: {actionName}{Environment.NewLine}调用参数: {args}";
            //Logger.Error(info, filterContext.Exception);

            var args = GetParasJson(filterContext);
            var info = $"调用: {filterContext.RequestContext.HttpContext.Request.Path}{Environment.NewLine}调用参数: {args}";
            
            Logger.Error(info, filterContext.Exception);
        }


        private static string GetParasJson(ExceptionContext filterContext)
        {
            return JsonEx.ToJson(filterContext.RequestContext.HttpContext.Request.Params);
        }
    }
}