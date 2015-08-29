using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Config
{
    /// <summary>
    /// 基本设置类
    /// </summary>
    public class BaseConfig
    {
        # region base method

        public static string ErrorMessage
        {
            get
            {
                return "请检查配置文件目录(config)中是否正确配置wkt.config文件";
            }
        }

        private static System.Timers.Timer baseConfigTimer = new System.Timers.Timer(60000);

        private static BaseConfigInfo m_configinfo;

        /// <summary>
        /// 静态构造函数初始化相应实例和定时器
        /// </summary>
        static BaseConfig()
        {
            m_configinfo = BaseConfigFileManager.LoadConfig();
            baseConfigTimer.AutoReset = true;
            baseConfigTimer.Enabled = true;
            baseConfigTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            baseConfigTimer.Start();
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ResetConfig();
        }


        /// <summary>
        /// 重设配置类实例
        /// </summary>
        public static void ResetConfig()
        {
            m_configinfo = BaseConfigFileManager.LoadConfig();
        }


        /// <summary>
        /// 重设配置类实例
        /// </summary>
        public static void ResetRealConfig()
        {
            m_configinfo = BaseConfigFileManager.LoadRealConfig();
        }

        public static BaseConfigInfo GetBaseConfig()
        {
            return m_configinfo;
        }

        /// <summary>
        /// 保存配置实例
        /// </summary>
        /// <param name="baseconfiginfo"></param>
        /// <returns></returns>
        public static bool SaveConfig(BaseConfigInfo baseconfiginfo)
        {
            BaseConfigFileManager acfm = new BaseConfigFileManager();
            BaseConfigFileManager.ConfigInfo = baseconfiginfo;
            return acfm.SaveConfig();
        }

        # endregion

        # region Config Property

        /// <summary>
        /// 可上传文件格式，以；分割
        /// </summary>
        public static string UploadFileExt
        {
            get
            {
                return GetBaseConfig().UploadFileExt;
            }
        }

        /// <summary>
        /// 服务IOC配置文件
        /// </summary>
        public static string ServiceUnityConfigPath
        {
            get
            {
                return GetBaseConfig().ServiceUnityConfigPath;
            }
        }

        /// <summary>
        /// 服务IOC配置文件
        /// </summary>
        public static string FacadeServiceUnityConfigPath
        {
            get
            {
                return GetBaseConfig().FacadeServiceUnityConfigPath;
            }
        }

        /// <summary>
        /// 业务逻辑IOC配置文件
        /// </summary>
        public static string BllUnityConfigPath
        {
            get
            {
                return GetBaseConfig().BllUnityConfigPath;
            }
        }

        # endregion
    }
}
