using System;

namespace QzMisBocHangZhou.Web.Filters
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Info级别日志记录者
        /// </summary>
        private static log4net.ILog s_InfoLogger = log4net.LogManager.GetLogger("InfoLog");

        /// <summary>
        /// Erro级别日志记录者
        /// </summary>
        private static log4net.ILog s_ErrorLogger = log4net.LogManager.GetLogger("ErrorLog");

        /// <summary>
        /// 普通信息日志
        /// </summary>
        /// <param name="message">日志信息对象</param>
        public static void Info(object message)
        {
            s_InfoLogger.Info(message);
        }


        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="exception">异常对象</param>
        public static void Error(Exception exception)
        {
            s_ErrorLogger.Error(exception);
        }


        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="exception">异常对象</param>
        public static void Error(object message, Exception exception)
        {
            s_ErrorLogger.Error(message, exception);
        }
    }
}