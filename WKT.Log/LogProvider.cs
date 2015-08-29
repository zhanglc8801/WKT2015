using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Log
{
    /// <summary>
    /// 日志提供者
    /// </summary>
    public sealed class LogProvider
    {
        private static WKT.Log.ILog _instance = new Log4NetLogger();

        /// <summary>
        /// 单例
        /// </summary>
        public static ILog Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
