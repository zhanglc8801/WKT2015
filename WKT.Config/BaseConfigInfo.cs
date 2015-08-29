using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Config
{
    /// <summary>
    /// 基本设置描述类, 加[Serializable]标记为可序列化
    /// </summary>
    [Serializable]
    public class BaseConfigInfo : IConfigInfo
    {
        /// <summary>
        /// 允许上传文件的扩展名
        /// </summary>
        public string UploadFileExt
        {
            get;
            set;
        }

        /// <summary>
        /// 服务IOC配置文件
        /// </summary>
        public string ServiceUnityConfigPath
        {
            get;
            set;
        }

        /// <summary>
        /// 服务IOC配置文件
        /// </summary>
        public string FacadeServiceUnityConfigPath
        {
            get;
            set;
        }

        /// <summary>
        /// 业务规则IOC配置文件
        /// </summary>
        public string BllUnityConfigPath
        {
            get;
            set;
        }
    }
}
