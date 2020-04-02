using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace QzMisBocHangZhou.Web.Filters
{
    /// <summary>
    /// Response拦截器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class ResponseFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 重写调用后请求(同步)
        /// </summary>
        /// <param name="actionExecutedContext">请求上下文操作</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext == null) throw new ArgumentNullException(nameof(actionExecutedContext));

            if (actionExecutedContext.Exception != null) return;

            var result = ReWriteResponseContent(actionExecutedContext);
            LoggerActionExecInfo(actionExecutedContext, result);
        }

        /// <summary>
        /// 重写输出流内容
        /// </summary>
        /// <param name="actionExecutedContext">请求上下文操作</param>
        /// <returns>统一返回值对象</returns>
        private static ResultModel<object> ReWriteResponseContent(HttpActionExecutedContext actionExecutedContext)
        {
            var data = FilterUtils.GetResponseContent(actionExecutedContext);
            var result = new ResultModel<object>(data);

            var status = GetResponseStatus(actionExecutedContext);
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(status, result);

            return result;
        }


        /// <summary>
        /// 获取输出响应状态
        /// </summary>
        /// <param name="actionExecutedContext">请求上下文操作</param>
        /// <returns>输出响应状态</returns>
        private static HttpStatusCode GetResponseStatus(HttpActionExecutedContext actionExecutedContext)
        {
            var status = actionExecutedContext.ActionContext.Response.StatusCode;
            if (status == HttpStatusCode.NoContent) status = HttpStatusCode.OK;

            return status;
        }


        /// <summary>
        /// 记录API请求日志
        /// </summary>
        /// <param name="actionExecutedContext">请求上下文操作</param>
        /// <param name="result">请求返回值</param>
        private static void LoggerActionExecInfo(HttpActionExecutedContext actionExecutedContext, ResultModel<object> result)
        {
            var actionName = FilterUtils.GetActionFullName(actionExecutedContext);
            var args = FilterUtils.GetRequestArgsJson(actionExecutedContext);
            var info = $"调用接口: {actionName}{Environment.NewLine}调用参数: {args}{Environment.NewLine}返回值: {JsonEx.ToJson(result)}";
            
            Logger.Info(info);
        }
    }
}