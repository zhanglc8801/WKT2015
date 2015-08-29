using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;

using log4net;
using log4net.Config;
using log4net.Appender;

namespace WKT.Log
{
    /// <summary>
    /// 日志管理类
    /// </summary>
    public class Log4NetLogger : WKT.Log.ILog
    {
        /// <summary>
        /// Debug
        /// </summary>
        public void Debug(string message)
        {
            LogManager.GetLogger((new StackTrace()).GetFrame(1).GetMethod().DeclaringType).Debug(message);
        }

        /// <summary>
        /// Info
        /// </summary>
        public void Info(string message)
        {
            LogManager.GetLogger((new StackTrace()).GetFrame(1).GetMethod().DeclaringType).Info(message);
        }

        /// <summary>
        /// Warn
        /// </summary>
        public void Warn(string message)
        {
            LogManager.GetLogger((new StackTrace()).GetFrame(1).GetMethod().DeclaringType).Warn(message);
        }

        /// <summary>
        /// Error
        /// </summary>
        public void Error(string message)
        {
            LogManager.GetLogger((new StackTrace()).GetFrame(1).GetMethod().DeclaringType).Error(message);
        }

        /// <summary>
        /// Fatal
        /// </summary>
        public void Fatal(string message)
        {
            LogManager.GetLogger((new StackTrace()).GetFrame(1).GetMethod().DeclaringType).Fatal(message);
        }

        /// <summary>
        /// 静态构造函数
        /// </summary>
        public Log4NetLogger()
        {
            string path = string.Format("{0}log4net.config", AppDomain.CurrentDomain.BaseDirectory);
            if (File.Exists(path))
            {
                log4net.Config.XmlConfigurator.Configure(new FileInfo(path));
            }
            else
            {
                RollingFileAppender appender = new RollingFileAppender();
                appender.Name = "root";
                appender.File = "log\\log.txt";
                appender.AppendToFile = true;
                appender.RollingStyle = RollingFileAppender.RollingMode.Composite;
                appender.DatePattern = "yyyyMMdd-HHmm\".txt\"";
                appender.MaximumFileSize = "1MB";
                appender.MaxSizeRollBackups = 10;
                log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout("%d{yyyy-MM-dd HH:mm:ss,fff}[%t] %-5p [%c] : %m%n");
                appender.Layout = layout;
                BasicConfigurator.Configure(appender);
                appender.ActivateOptions();
            }
        }
    }
}