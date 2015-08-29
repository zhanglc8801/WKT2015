using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Log
{
    /// <summary>
    /// 应用程序日志记录
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">要记录的信息</param>
        void Info(string message);
        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message">要记录的信息</param>
        void Warn(string message);
        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="message">要记录的信息</param>
        void Debug(string message);
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message">要记录的信息</param>
        void Error(string message);
        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="message">要记录的信息</param>
        void Fatal(string message);

    }
}
