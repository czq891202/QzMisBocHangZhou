using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace QzMisBocHangZhou.Web.Filters
{
    /// <summary>
    /// 自定义过滤器工具类
    /// </summary>
    public static class FilterUtils
    {
        /// <summary>
        /// 获取请求名称
        /// </summary>
        /// <param name="actionExecutedContext">请求上下文操作</param>
        /// <returns>请求接口的完整名称</returns>
        public static string GetActionFullName(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext == null) throw new ArgumentNullException(nameof(actionExecutedContext), "请求上下文操作不可为空!");

            return GetActionFullName(actionExecutedContext.ActionContext);
        }

        /// <summary>
        /// 获取请求名称
        /// </summary>
        /// <param name="actionContext">请求上下文操作</param>
        /// <returns>请求接口的完整名称</returns>
        public static string GetActionFullName(HttpActionContext actionContext)
        {
            if (actionContext == null) throw new ArgumentNullException(nameof(actionContext), "请求上下文操作不可为空!");

            return $"{actionContext.ActionDescriptor.ControllerDescriptor.ControllerName}/{actionContext.ActionDescriptor.ActionName}";
        }


        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="actionExecutedContext">请求上下文操作</param>
        /// <returns>Json格式的请求对象参数</returns>
        public static string GetRequestArgsJson(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext == null) throw new ArgumentNullException(nameof(actionExecutedContext), "请求上下文操作不可为空!");

            return GetRequestArgsJson(actionExecutedContext.ActionContext);
        }

        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="actionContext">请求上下文操作</param>
        /// <returns>Json格式的请求对象参数</returns>
        public static string GetRequestArgsJson(HttpActionContext actionContext)
        {
            if (actionContext == null) throw new ArgumentNullException(nameof(actionContext), "请求上下文操作不可为空!");

            return JsonEx.ToJson(actionContext.ActionArguments);
        }

        /// <summary>
        /// 获取请求返回输出内容
        /// </summary>
        /// <param name="actionExecutedContext">请求上下文操作</param>
        /// <returns>输出内容对象</returns>
        public static object GetResponseContent(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext == null) throw new ArgumentNullException(nameof(actionExecutedContext), "请求上下文操作不可为空!");

            return GetResponseContent(actionExecutedContext.ActionContext);
        }

        /// <summary>
        /// 获取请求返回输出内容
        /// </summary>
        /// <param name="actionContext">请求上下文操作</param>
        /// <returns>输出内容对象</returns>
        public static object GetResponseContent(HttpActionContext actionContext)
        {
            if (actionContext == null) throw new ArgumentNullException(nameof(actionContext), "请求上下文操作不可为空!");

            return actionContext.Response.Content?.ReadAsAsync<object>().Result;
        }
    }
}